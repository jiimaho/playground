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
    public void Post([FromBody] DisasterRequest disaster)
    {
        using var db = new DisastersDbContext();
        var locations = disaster.Locations.Select(l => new Location
        {
            Country = l.Country,
            LocationId = l.LocationId
        }).ToList();
        
        var disasterEntity = new Disaster
        {
            DisasterId = Guid.NewGuid(),
            Summary = disaster.Summary,
            Locations = locations
        };

        db.Locations.AddRange(locations);
        db.Disasters.Add(disasterEntity);
        db.SaveChanges();
    }

    [HttpGet(Name = "GetDisasters")]
    public DisastersResponse Get()
    {
        using var db = new DisastersDbContext();
        var disasters = db.Disasters
            .Include(x => x.Locations)
            .AsNoTracking()
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
                    Locations = x.Locations.Select(p => new LocationResponseItem
                    {
                        LocationId = p.LocationId,
                        Country = p.Country
                    })
                })
        };
    }
}