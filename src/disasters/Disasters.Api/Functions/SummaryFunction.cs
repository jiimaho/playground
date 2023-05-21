using Disasters.Api.Db;
using Microsoft.EntityFrameworkCore;

namespace Disasters.Api.Functions;

public class SummaryFunction
{
    public record SummaryResponse(IEnumerable<DisasterSummaryResponseItem> disasters);
    public record DisasterSummaryResponseItem(string Summary, string Places);
    
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