namespace Healthy.Web.Services;

public interface IReportService
{
    Task Report(ReportDto report);
}

public class ReportService(HealthyDbContext dbContext) : IReportService
{
    public async Task Report(ReportDto reportDto)
    {
        var report = new Report
        {
            Text = reportDto.text,
            Mood = (Mood)reportDto.mood
        };
        dbContext.Add(report);
        await dbContext.SaveChangesAsync();
    }
}