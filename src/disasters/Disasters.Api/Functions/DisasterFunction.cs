using System.Diagnostics;
using System.Diagnostics.Metrics;
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
    private readonly ActivitySource _activity; // Equivalent to RootSpan in OpenTelemetry
    private readonly Meter _meter; // Equivalent to Meter in OpenTelemetry
    private readonly Counter<int> _getCounter;
    private readonly Histogram<int> _responseCounter;

    public DisasterFunction(ILogger<DisasterFunction> logger)
    {
        _activity = new ActivitySource("Disasters.Api", "1.0.0");
        _meter = new Meter("Disasters.Api", "1.0.0");
        _getCounter = _meter.CreateCounter<int>("number_of_get_requests", "requests", "Number of GET requests made to the Get Lambda function");
        _responseCounter = _meter.CreateHistogram<int>("response_time", "milliseconds", "Response time in milliseconds for the Get Lambda function");
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
    
    public async Task<APIGatewayProxyResponse> Post(APIGatewayProxyRequest apiRequest, ILambdaContext context)
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

    public async Task<APIGatewayProxyResponse> Get(APIGatewayProxyRequest request, ILambdaContext context)
    {
        using (var activity = _activity.CreateActivity(nameof(Get), ActivityKind.Internal))
        {
            await using var db = new DisastersDbContext();
            var disastersQuery = db.Disasters
                .Include(x => x.DisasterLocations)
                .ThenInclude(x => x.Location)
                .AsNoTracking();

            IEnumerable<Disaster> disasters;
            using (var activity2 = _activity.CreateActivity("Get from db", ActivityKind.Internal))
            {
                disasters = disastersQuery
                    .OrderBy(p => p.Occured)
                    .ToList();
            }

            DisastersResponse response;
            using (var activity3 = _activity.CreateActivity("Map to response", ActivityKind.Internal))
            {
                response = new DisastersResponse
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
            }
            activity.Stop();
            _responseCounter.Record(activity.Duration.Milliseconds);
            _getCounter.Add(1);
            
            return new APIGatewayProxyResponse();
        }
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