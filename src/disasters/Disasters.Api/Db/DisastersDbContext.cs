using Disasters.Api.Db.Seed;
using Microsoft.EntityFrameworkCore;

namespace Disasters.Api.Db;

public class DisastersDbContext : AuditableDisastersDbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
                // For when running locally
                // "Server=localhost,1433;Database=disastersql;User Id=sa;Password=V34fxzM6xfVBu23ALLxc;Encrypt=False",
                // For when running in docker
                "Server=172.17.0.2,1433;Database=disastersql;User Id=sa;Password=V34fxzM6xfVBu23ALLxc;Encrypt=False",
                sqlServerOptionsBuilder =>
                {
                    sqlServerOptionsBuilder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null);
                    sqlServerOptionsBuilder.CommandTimeout(20);
                })
            .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
            .EnableSensitiveDataLogging();
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().HaveMaxLength(300);
        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestView>().HasNoKey().ToView("TestView");

        modelBuilder.ApplyConfiguration(new LocationConfiguration());
        modelBuilder.ApplyConfiguration(new DisasterConfiguration());
        modelBuilder.ApplyConfiguration(new DisasterLocationSeed());
        
        DefaultForeignKeysToRestrictBehaviourOnDelete(modelBuilder);
    }

    private static void DefaultForeignKeysToRestrictBehaviourOnDelete(ModelBuilder modelBuilder)
    {
        var foreignKeys = modelBuilder.Model.GetEntityTypes()
            .SelectMany(e => e.GetForeignKeys())
            .Where(x => x is { IsOwnership: false, DeleteBehavior: DeleteBehavior.Cascade });
        
        foreach (var mutableForeignKey in foreignKeys)
        {
            mutableForeignKey.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    public DbSet<Disaster> Disasters { get; set; }
    
    public DbSet<Location> Locations { get; set; }

    public DbSet<DisasterLocation> DisasterLocations { get; set; }

    public DbSet<TestView> TestView { get; set; }
}