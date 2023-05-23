using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disasters.Api.Db.Seed;

public class DisasterConfiguration : IEntityTypeConfiguration<Disaster>
{
    public void Configure(EntityTypeBuilder<Disaster> builder)
    {
        builder.Property(p => p.Summary).HasMaxLength(300);
        builder.HasIndex(p => p.Occured);
        builder.ToTable("Disasters", tableBuilder => tableBuilder.IsTemporal());
        // Just for demonstration purpose. How to create a index with multiple columns
        // builder.HasIndex(p => new { p.Summary, p.Occured}).IsUnique();
        
        builder.HasData(new Disaster
        {
            DisasterId = Guid.Parse("694be870-2024-41a5-b08a-5054b431b4c2"),
            Occured = DateTimeOffset.Now,
            Summary = "Seed"
        });
    }
}