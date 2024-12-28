targetScope = 'resourceGroup'
param location string
param name string = 'scacrplayground'

resource acrPlayground 'Microsoft.ContainerRegistry/registries@2021-09-01' = {
  name: name
  location: location
  sku: {
    name: 'Basic' 
  }
  properties: {
    adminUserEnabled: true 
  }
}
