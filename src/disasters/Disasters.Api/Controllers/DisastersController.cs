using Disasters.Api.Db;
using Microsoft.AspNetCore.Mvc;

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
    
    [HttpPost(Name = "AddDisaster")]
    public void Post([FromBody] DisasterRequest disaster)
    {
        using (var db = new DisastersDbContext())
        {
            var disasterEntity = new Disaster
            {
                DisasterId = Guid.NewGuid(),
                Summary = disaster.Summary
            };
            db.Disasters.Add(disasterEntity);
            db.SaveChanges();
        }
    }

    [HttpGet(Name = "GetDisasters")]
    public DisastersResponse Get()
    {
        using (var db = new DisastersDbContext())
        {
            
        }
        
        return new DisastersResponse
        {
            Disasters = new List<DisasterResponseItem>
            {
                new DisasterResponseItem
                {
                    Date = DateOnly.FromDateTime(DateTime.UtcNow),
                    Summary = "Shit in here"
                }
            }
        };
    }
}