using Microsoft.EntityFrameworkCore;

namespace Healthy.Web;

public class HealthyDbContext : DbContext
{
   public DbSet<Report> Reports { get; set; }

   public string DbPath { get; }

   public HealthyDbContext()
   {
       var folder = Environment.SpecialFolder.LocalApplicationData;
       var path = Environment.GetFolderPath(folder);
       DbPath = System.IO.Path.Join(path, "blogging.db");
   }

   // The following configures EF to create a Sqlite database file in the
   // special "local" folder for your platform.
   protected override void OnConfiguring(DbContextOptionsBuilder options)
       => options.UseSqlite($"Data Source={DbPath}");
}

public class Report
{
    public int Id { get; set; }
    public string Text { get; set; }
    public Mood Mood { get; set; }
}

public enum Mood
{
    VeryBad,
    Bad,
    Good,
    VeryGood
}