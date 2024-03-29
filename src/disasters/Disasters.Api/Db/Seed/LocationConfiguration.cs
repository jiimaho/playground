using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disasters.Api.Db.Seed;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.Property(p => p.Country).HasMaxLength(50);
        
        builder.HasData(new List<Location>
        {
            new()
            {
                Country = "Sweden",
                LocationId = Guid.Parse("49c6030e-9dd8-4155-8f65-47394943b804")
            }
        });
    }
}