using Disasters.Api.Db;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Disasters.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SummaryController : ControllerBase
{
    public record SummaryResponse(IEnumerable<DisasterSummaryResponseItem> disasters);
    public record DisasterSummaryResponseItem(string Summary, string Places);
    
    [HttpGet(Name = "GetSummary")]
    public SummaryResponse GetSummary()
    {
        using var db = new DisastersDbContext();
        var disasters = db.Disasters
            .Include(x => x.DisasterLocations)
            .ThenInclude(x => x.Location)
            .AsNoTracking()
            .Select(x => new
            {
                x.Summary,
                Places = string.Join(',', x.DisasterLocations.Select(p => p.Location.Country).Distinct())
            })
            .ToList();
        
        return new SummaryResponse(disasters.Select(x => new DisasterSummaryResponseItem(x.Summary, x.Places)));
    }
}