using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Disasters.Api.Db.Seed;

public class DisasterSeed : IEntityTypeConfiguration<Disaster>
{
    public void Configure(EntityTypeBuilder<Disaster> builder)
    {
        builder.HasData(new Disaster
        {
            DisasterId = Guid.Parse("694be870-2024-41a5-b08a-5054b431b4c2"),
            Occured = DateTimeOffset.Now,
            Summary = "Seed"
        });
    }
}