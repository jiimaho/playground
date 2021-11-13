// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

Console.WriteLine("1. Instantiating an AmazonDynamoDBClient");
using IAmazonDynamoDB client = new AmazonDynamoDBClient(RegionEndpoint.EUNorth1);

Console.WriteLine("2. Creating the actual query");
// ReSharper disable once StringLiteralTypo
var query = new QueryRequest("customertestjim")
{
    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
    {
        { ":Id", new AttributeValue { S = "12345" }}
    },
    KeyConditionExpression = "CustomerId = :Id"
};

Console.WriteLine("3. Issuing query");
var queryResponse = await client.QueryAsync(query);

var row = queryResponse.Items.Single();

Console.WriteLine("4. Parsing the query response into a new anonymous object");
var customer = new { CustomerId = row["CustomerId"].S, Name = row["Name"].S };
    
Console.WriteLine($"5. Got a customer with id {customer.CustomerId} and name {customer.Name}");
Console.WriteLine("Done! Disposal of the client will now happen");
Console.WriteLine("Exiting");