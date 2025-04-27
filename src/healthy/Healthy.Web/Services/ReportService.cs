using Healthy.Web.Storage;

namespace Healthy.Web.Services;

public class ReportService(DatabaseContext dbContext) : IReportService
{

    public async Task Report(ActivityReportDto activityReport)
    {
        var model = new ActivityReportModel
        {
            Id = activityReport.Id,
            Activity = activityReport.Activity,
            ActivityType = (ActivityTypeModel)activityReport.ActivityType,
            FeelingType = (FeelingTypeModel)activityReport.FeelingType,
            Time = activityReport.Time
        };
    
        await dbContext.ActivityReports.AddAsync(model);
        await dbContext.SaveChangesAsync();
 
    }
}