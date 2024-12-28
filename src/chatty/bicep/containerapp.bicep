targetScope = 'resourceGroup'
param location string = resourceGroup().location
param environmentName string = 'chatty-dev'
param webAppName string = 'chatty-web'
param siloAppName string = 'chatty-silo'
param webAppImage string = 'chatty-web-amd64:1'
param siloAppImage string = 'chatty-silo-amd64:7'
param registryName string = 'scacrplayground'

// Log Analytics workspace
resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
    name: '${environmentName}-logs'
    location: location
    properties: {
        retentionInDays: 30
        sku: {
            name: 'PerGB2018'
        }
    }
}

// Container Apps Environment
resource environment 'Microsoft.App/managedEnvironments@2023-05-01' = {
    name: environmentName
    location: location
    properties: {
        appLogsConfiguration: {
            destination: 'log-analytics'
            logAnalyticsConfiguration: {
                customerId: logAnalyticsWorkspace.properties.customerId
                sharedKey: logAnalyticsWorkspace.listKeys().primarySharedKey
            }
        }
    }
}

// User assigned managed identity
resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2023-01-31' = {
    name: 'chatty-identity'
    location: location
}

module cosmosDb 'cosmos.bicep' = {
    name: 'cosmos-db'
    scope: resourceGroup('rg-playground-acr')
    params: {
        location: location
        existingManagedIdentityPrincipalId: managedIdentity.properties.principalId
    }
}

// Reference to existing ACR
resource acr 'Microsoft.ContainerRegistry/registries@2024-11-01-preview' existing = {
    name: registryName
}

// Assign AcrPull role to the managed identity
resource acrPullRole 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
    name: guid(acr.id, managedIdentity.id, '7f951dda-4ed3-4680-a7ca-43fe172d538d')
    scope: acr
    properties: {
        principalId: managedIdentity.properties.principalId
        roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', '7f951dda-4ed3-4680-a7ca-43fe172d538d') // AcrPull role
        principalType: 'ServicePrincipal'
    }
}

// Chatty silo 
resource siloAppContainer 'Microsoft.App/containerApps@2023-05-01' = {
    name: siloAppName
    location: location
    identity: {
        type: 'UserAssigned'
        userAssignedIdentities: {
            '${managedIdentity.id}': {}
        }
    }
    dependsOn: [
        acrPullRole
    ]
    properties: {
        managedEnvironmentId: environment.id
        configuration: {
            registries: [
                {
                    server: '${registryName}.azurecr.io'
                    identity: managedIdentity.id
                }
            ]
            ingress: {
                external: true
                targetPort: 8080 
            }
        }
        template: {
            containers: [
                {
                    name: siloAppName
                    image: '${registryName}.azurecr.io/${siloAppImage}'
                    resources: {
                        cpu: json('0.5')
                        memory: '1Gi'
                    }
                    env: [
                        {
                            name: 'COSMOS_ENDPOINT'
                            value: cosmosDb.outputs.cosmosEndpoint
                        }
                        {
                            name: 'ASPNETCORE_ENVIRONMENT'
                            value: 'Production'
                        }
                        {
                            name: 'SILO_NUMBER'
                            value: '1'
                        }
                        {
                            name: 'GATEWAY_PORT'
                            value: '30000'
                        }
                        {
                            name: 'SILO_PORT'
                            value: '11111'
                        }
                        {
                            name: 'MANAGED_IDENTITY_CLIENT_ID'
                            value: managedIdentity.properties.clientId
                        }
                    ]
                }
            ]
            scale: {
                minReplicas: 1
                maxReplicas: 1
            }
        }
    }
}

// Chatty web 
resource chattyAppContainer 'Microsoft.App/containerApps@2023-05-01' = {
    name: webAppName
    location: location
    identity: {
        type: 'UserAssigned'
        userAssignedIdentities: {
            '${managedIdentity.id}': {}
        }
    }
    dependsOn: [
        acrPullRole
    ]
    properties: {
        managedEnvironmentId: environment.id
        configuration: {
            registries: [
                {
                    server: '${registryName}.azurecr.io'
                    identity: managedIdentity.id 
                }
            ]
            ingress: {
                external: true
                targetPort: 80
            }
        }
        template: {
            containers: [
                {
                    name: webAppName
                    image: '${registryName}.azurecr.io/${webAppImage}'
                    resources: {
                        cpu: json('0.5')
                        memory: '1Gi'
                    }
                    env: [
                        {
                            name: 'COSMOS_ENDPOINT'
                            value: cosmosDb.outputs.cosmosEndpoint 
                        }
                        {
                            name: 'ASPNETCORE_ENVIRONMENT'
                            value: 'Production'
                        }
                    ]
                }
            ]
            scale: {
                minReplicas: 1
                maxReplicas: 1
            }
        }
    }
}

output webContainerAppFQDN string = chattyAppContainer.properties.configuration.ingress.fqdn
output siloContainerAppFQDN string = siloAppContainer.properties.configuration.ingress.fqdn