using Microsoft.EntityFrameworkCore;

namespace Healthy.Web.Storage;

public class DatabaseContext(DbContextOptions<DatabaseContext> options): DbContext(options)
{
   public DbSet<ActivityReportModel> ActivityReports { get; set; }
}