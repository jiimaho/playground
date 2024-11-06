namespace Orleans.Silo.Configuration.Serialization.Surrogate;

[RegisterConverter]
public class NodaTimeSurrogateConverter : IConverter<object, NodaTimeSurrogate>
{
    public object ConvertFromSurrogate(in NodaTimeSurrogate surrogate)
    {
        throw new NotImplementedException();
    }

    public NodaTimeSurrogate ConvertToSurrogate(in object value)
    {
        throw new NotImplementedException();
    }
}