using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disasters.Api.Db.Seed;

public class DisasterLocationSeed : IEntityTypeConfiguration<DisasterLocation>
{
    public void Configure(EntityTypeBuilder<DisasterLocation> builder)
    {
        builder.HasData(new List<DisasterLocation>
        {
            new()
            {
                DisasterLocationId = Guid.Parse("8feb0a12-5317-490c-a4a4-3d8d5c1328c8"),
                LocationId = Guid.Parse("49c6030e-9dd8-4155-8f65-47394943b804"),
                DisasterId = Guid.Parse("694be870-2024-41a5-b08a-5054b431b4c2")
            }
        });
    }
}