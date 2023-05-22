using System.Text.Json;
using System.Text.Json.Serialization;
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
    
    public record DeleteDisasterRequest(Guid DisasterId);
    
    public void Delete(DeleteDisasterRequest request)
    {
        using var db = new DisastersDbContext();
        var disaster = db.Disasters.Find(request.DisasterId);
        if (disaster == null) return;
        db.Disasters.Remove(disaster);
        db.SaveChanges();
    }
    
    public void Post(DisasterRequest request, ISystemClock clock)
    {
        using var db = new DisastersDbContext();
        
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

        db.Locations.AddRange(locations.Select(x => x.Location));
        db.DisasterLocations.AddRange(locations);
        db.Disasters.Add(disasterEntity);
        db.SaveChangesAsync("Jim").Wait();
    }

    public static async Task<APIGatewayProxyResponse> Get(APIGatewayProxyRequest request, ILambdaContext context)
    {
        Console.WriteLine("Hello World");
        return new APIGatewayProxyResponse
        {
            Body = "This is a test",
            StatusCode = 200
        };
        using var db = new DisastersDbContext();
        var disastersQuery = db.Disasters
            .Include(x => x.DisasterLocations)
            .ThenInclude(x => x.Location)
            .AsNoTracking();

        // if (fromDate != default)
        // {
        //     disastersQuery = disastersQuery.Where(x => x.Occured >= fromDate.BeginningOfDay());
        // }
            
        var disasters = disastersQuery
            .OrderBy(p => p.Occured)
            .ToList();
            
        var dis = new DisastersResponse
        {
            Disasters = disasters.Select(x => 
                new DisasterResponseItem
                {
                    Summary = x.Summary,
                    Occured = x.Occured,
                    DisasterId = x.DisasterId,
                    Locations = x.DisasterLocations.Select(p => new LocationResponseItem
                    {
                        LocationId = p.Location.LocationId,
                        Country = p.Location.Country
                    })
                })
        };

        return new APIGatewayProxyResponse();
    }
}