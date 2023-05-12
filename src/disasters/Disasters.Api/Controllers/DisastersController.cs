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

    [HttpGet(Name = "GetDisasters")]
    public DisastersResponse Get()
    {
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