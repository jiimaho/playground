using Disasters.Api.Db.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Disasters.Api.Db;

public class DisastersDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
                "Server=localhost,1433;Database=disastersql;User Id=sa;Password=V34fxzM6xfVBu23ALLxc;Encrypt=False")
            // "Data Source=(localhost)\\disastersql;Initial Catalog=Disasters;Encrypt=False");
            .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestView>().HasNoKey().ToView("TestView");
        
        modelBuilder.ApplyConfiguration(new LocationSeed());
        modelBuilder.ApplyConfiguration(new DisasterSeed());
        modelBuilder.ApplyConfiguration(new DisasterLocationSeed());
    }

    public DbSet<Disaster> Disasters { get; set; }
    
    public DbSet<Location> Locations { get; set; }

    public DbSet<DisasterLocation> DisasterLocations { get; set; }

    public DbSet<TestView> TestView { get; set; }
}