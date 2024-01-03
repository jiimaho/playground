namespace Grains.Implementation;

[GenerateSerializer]
[Alias("WallboxState")]
public class WallboxState
{
    [Id(0)] public WallboxStatus Status { get; set; } = WallboxStatus.Unavailable;
}