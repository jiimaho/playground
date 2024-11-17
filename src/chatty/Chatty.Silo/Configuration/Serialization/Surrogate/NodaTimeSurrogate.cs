namespace Chatty.Silo.Configuration.Serialization.Surrogate;

[Alias("NodaTimeSurrogate")]
[GenerateSerializer]
public struct NodaTimeSurrogate
{
    [Id(0)]
    public int Test { get; set; }
}