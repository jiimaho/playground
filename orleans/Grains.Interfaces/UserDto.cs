namespace Grains;

[GenerateSerializer]
[Alias("UserDto")]
public record UserDto
{
    [Id(0)]
    public string Name { get; set; }
};