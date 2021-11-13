// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

using IAmazonDynamoDB client = new AmazonDynamoDBClient(RegionEndpoint.EUNorth1);

// ReSharper disable once StringLiteralTypo
var query = new QueryRequest("customertestjim") // This is the name of the table in our DynamoDb
{
    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
    {
        { ":Id", new AttributeValue { S = "12345" }}
    },
    KeyConditionExpression = "CustomerId = :Id"
};

var queryResponse = await client.QueryAsync(query);

var row = queryResponse.Items.Single();

var customer = new { CustomerId = row["CustomerId"].S, Name = row["Name"].S };
    
Console.WriteLine($"5. Got a customer with id {customer.CustomerId} and name {customer.Name}");
Console.WriteLine("Done! Disposal of the client will now happen");
Console.WriteLine("Exiting");