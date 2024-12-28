targetScope = 'subscription'
param location string

resource resourceGroup 'Microsoft.Resources/resourceGroups@2020-06-01' = {
  name: 'rg-playground-acr'
  location: location
}