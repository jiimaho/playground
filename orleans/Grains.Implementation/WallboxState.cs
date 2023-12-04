namespace Grains.Implementation;

[GenerateSerializer]
[Alias("WallboxState")]
public class WallboxState
{
    [Id(0)] public string Status { get; set; } = "Unknown";
}