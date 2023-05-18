using Disasters.Api.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Disasters.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DisastersController : ControllerBase
{
    private readonly ILogger<DisastersController> _logger;

    public DisastersController(ILogger<DisastersController> logger)
    {
        _logger = logger;
    }
    
    public record DeleteDisasterRequest(Guid DisasterId);
    
    [HttpDelete(Name = "DeleteDisaster")]
    public void Delete([FromBody] DeleteDisasterRequest request)
    {
        using var db = new DisastersDbContext();
        var disaster = db.Disasters.Find(request.DisasterId);
        if (disaster == null) return;
        db.Disasters.Remove(disaster);
        db.SaveChanges();
    }
    
    [HttpPost(Name = "AddDisaster")]
    public void Post([FromBody] DisasterRequest request, ISystemClock clock)
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
        db.SaveChanges();
    }

    [HttpGet(Name = "GetDisasters")]
    public DisastersResponse Get([FromQuery] DateOnly fromDate)
    {
        using var db = new DisastersDbContext();
        var disastersQuery = db.Disasters
            .Include(x => x.DisasterLocations)
            .ThenInclude(x => x.Location)
            .AsNoTracking();

        if (fromDate != default)
        {
            disastersQuery = disastersQuery.Where(x => x.Occured >= fromDate.BeginningOfDay());
        }
            
        var disasters = disastersQuery
            .OrderBy(p => p.Occured)
            .ToList();
        // var disasters = db.Disasters.Where(x => EF.Functions.Like(x.Summary, "%STUFF%")).ToList();
            
        return new DisastersResponse
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
    }
    
    
}