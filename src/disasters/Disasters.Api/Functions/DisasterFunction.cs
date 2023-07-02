using System.Text;
using System.Text.Json;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Disasters.Api.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Disasters.Api.Functions;

public class DisasterFunction
{
    private readonly ILogger<DisasterFunction> _logger;

    public DisasterFunction(ILogger<DisasterFunction> logger)
    {
        _logger = logger;
    }

    public async Task<APIGatewayProxyResponse> Delete(APIGatewayProxyRequest apiRequest, ILambdaContext context)
    {
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(apiRequest.Body));
        var request = await JsonSerializer.DeserializeAsync<DeleteDisasterRequest>(memoryStream);
        
        await using var db = new DisastersDbContext();
        var disaster = await db.Disasters.FindAsync(request.DisasterId);
        if (disaster == null) return new APIGatewayProxyResponse { StatusCode = 404 };
        db.Disasters.Remove(disaster);
        await db.SaveChangesAsync();
        return new APIGatewayProxyResponse{ StatusCode = 200 };
    }
    
    public static async Task<APIGatewayProxyResponse> Post(APIGatewayProxyRequest apiRequest, ILambdaContext context)
    {
        Console.WriteLine("Entering DisasterFunction.Post");
        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(apiRequest.Body));
        var request = await JsonSerializer.DeserializeAsync<DisasterRequest>(memoryStream);
        Console.WriteLine("Deserialized");
        Console.WriteLine(apiRequest.Body);

        var disasterEntity = new Disaster
        {
            DisasterId = Guid.NewGuid(),
            Occured = request.Occured,
            Summary = request.Summary
        };

        var locations = request.Locations.Select(p => new DisasterLocation
        {
            Disaster = disasterEntity,
            DisasterLocationId = Guid.NewGuid(),
            Location = new Location
            {
                Country = p.Country,
                LocationId = p.LocationId
            }
        }).ToList();


        Console.WriteLine("Entities created");
        await using var db = new DisastersDbContext();
        // Just for demonstration: using transaction
        var strategy = db.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync<Disaster, Disaster>(null, async (c, state, token) =>
            {
                await using var transaction = await db.Database.BeginTransactionAsync(token);
                Console.WriteLine("BeginTransactionAsync");
                try
                {
                    db.Locations.AddRange(locations.Select(x => x.Location));
                    db.DisasterLocations.AddRange(locations);
                    db.Disasters.Add(disasterEntity);

                    await db.SaveChangesAsync(token);
                    await transaction.CommitAsync(token);
                    Console.WriteLine("Committed");
                    Console.WriteLine("Changes committed");
                    return disasterEntity;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception caught");
                    Console.WriteLine(e);
                    await transaction.RollbackAsync(token);
                    throw;
                }
            }, verifySucceeded: null);

        return new APIGatewayProxyResponse
        {
            StatusCode = 200
        };
    }

    public static async Task<APIGatewayProxyResponse> Get(APIGatewayProxyRequest request, ILambdaContext context)
    {
        Console.WriteLine("This indeed seem to work!");
        await using var db = new DisastersDbContext();
        var disastersQuery = db.Disasters
            .Include(x => x.DisasterLocations)
            .ThenInclude(x => x.Location)
            .AsNoTracking();
        
        var disasters = disastersQuery
            .OrderBy(p => p.Occured)
            .ToList();
        
        var response = new DisastersResponse
        {
            Disasters = disasters.Select(x =>
                new DisasterResponseItem(
                    Summary: x.Summary, 
                    Occured: x.Occured, 
                    DisasterId: x.DisasterId,
                    Locations: x.DisasterLocations.Select(p =>
                        new LocationResponseItem(
                            LocationId: p.Location.LocationId, 
                            Country: p.Location.Country))))
        };
        
        return new APIGatewayProxyResponse();
    }
}

public record DeleteDisasterRequest(Guid DisasterId);

public record DisasterRequest
{
    public DateTimeOffset Occured { get; set; }
    public string Summary { get; set; } = "";
    public IEnumerable<LocationRequest> Locations { get; set; } = new List<LocationRequest>();
}

public record DisastersResponse
{
    public IEnumerable<DisasterResponseItem> Disasters { get; set; } = new List<DisasterResponseItem>();
}

public record DisasterResponseItem(
    Guid DisasterId,
    DateTimeOffset Occured,
    string Summary,
    IEnumerable<LocationResponseItem> Locations);

public record LocationResponseItem(Guid LocationId, string Country);

public record LocationRequest(Guid LocationId, string Country);