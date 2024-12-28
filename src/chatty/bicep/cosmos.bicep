// General
param location string = resourceGroup().location
param cosmosContainerDefaultThroughput int = 400

// Managed Identity
param existingManagedIdentityPrincipalId string

// Cosmos DB
param cosmosAccountName string = 'cosmos-scplayground'
param cosmosDatabaseName string = 'chatty'

// Cosmos DB - Silo
param siloContainerName string = 'silo'

// Cosmos DB - State
param stateContainerName string = 'state'

resource cosmosAccount 'Microsoft.DocumentDB/databaseAccounts@2024-02-15-preview' = {
    name: cosmosAccountName
    location: location
    properties: {
        databaseAccountOfferType: 'Standard'
        locations: [
            {
                locationName: location
                failoverPriority: 0
            }
        ]
        consistencyPolicy: {
            defaultConsistencyLevel: 'Session'
        }
    }
}

resource database 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2024-02-15-preview' = {
    parent: cosmosAccount
    name: cosmosDatabaseName
    properties: {
        resource: {
            id: cosmosDatabaseName
        }
    }
}

// Cosmos container for silo clustering
resource siloCosmosContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2024-02-15-preview' = {
    parent: database
    name: siloContainerName
    properties: {
        resource: {
            id: siloContainerName
            partitionKey: {
                paths: [
                   '/ClusterId' 
                ]
                kind: 'Hash'
            }
            indexingPolicy: {
                indexingMode: 'consistent'
                includedPaths: [
                    { path: '/*' }
                ]
                excludedPaths: [
                    { path: '/"_etag"/?' }
                ]
            }
            options: {
                throughput: cosmosContainerDefaultThroughput
            }
            sku: {
                name: 'Standard'
            }
        }
    }
}

// Cosmos container for state 
resource stateCosmosContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2024-02-15-preview' = {
    parent: database
    name: stateContainerName
    properties: {
        resource: {
            id: stateContainerName
            partitionKey: {
                paths: [
                   '/GrainReference' 
                ]
                kind: 'Hash'
            }
            indexingPolicy: {
                indexingMode: 'consistent'
                includedPaths: [
                    { path: '/*' }
                ]
                excludedPaths: [
                    { path: '/"_etag"/?' }
                ]
            }
            options: {
                throughput: cosmosContainerDefaultThroughput
            }
            sku: {
                name: 'Standard'
            }
        }
    }
}

// NOTE TO SELF: if this is not enough then try add another role definition az cosmosdb sql role assignment create --account-name cosmos-scplayground --resource-group rg-playground-acr --scope "/" --principal-id 8c619f71-3046-4793-9865-98b6212d9271 --role-definition-name "Cosmos DB Built-in Data Contributor"
// Add role assignment for container app managed identity
resource dbContributorRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
    name: guid(cosmosAccount.id, existingManagedIdentityPrincipalId, 'scsql-role')
    properties: {
        principalId: existingManagedIdentityPrincipalId
        roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', 'b24988ac-6180-42a0-ab88-20f7382dd24c') // Cosmos DB Data Contributor role
        principalType: 'ServicePrincipal'
    }
    scope: cosmosAccount
}

// Add role assignment for container app managed identity
// resource roleAssignmentMore 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
//     name: guid(cosmosAccount.id, existingManagedIdentityPrincipalId, 'scsql-role-more')
//     properties: {
//         principalId: existingManagedIdentityPrincipalId
//         roleDefinitionId: customRoleDefinition.id
//         principalType: 'ServicePrincipal'
//     }
//     scope: cosmosAccount
// }
// 
// resource customRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-04-01' = {
//     name: guid(subscription().id, 'ScCustomRoleWrite') // Generate a unique name for the role
//     properties: {
//         roleName: 'Cosmos DB Database and Container Creator'
//         description: 'Allows creation of databases and containers in Azure Cosmos DB'
//         assignableScopes: [
//             cosmosAccount.id
//         ]
//         permissions: [
//             {
//                 actions: [
//                     'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/write' // Create or update databases
//                     'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/write' // Create or update containers
//                     'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/read' // Read databases
//                     'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/read' // Read containers
//                 ]
//                 notActions: []
//                 dataActions: [
//                     'Microsoft.DocumentDB/databaseAccounts/*'
//                 ]
//                 notDataActions: []
//             }
//         ]
//     }
// }

output cosmosEndpoint string = cosmosAccount.properties.documentEndpoint