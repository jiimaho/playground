namespace Healthy.Web.Services;

public interface IReportService
{
   public Task Report(ActivityReportDto activityReport);
}