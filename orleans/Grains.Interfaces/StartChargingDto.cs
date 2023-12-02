namespace Grains;

[GenerateSerializer]
[Alias("StartChargingDto")]
public record StartChargingDto
{
    [Id(0)]
    public UserDto User { get; set; }
    
    [Id(1)]
    public int Current { get; set; }
};