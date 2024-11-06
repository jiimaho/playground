using System.Buffers;
using NodaTime;
using Orleans.Serialization.Buffers;
using Orleans.Serialization.Cloning;
using Orleans.Serialization.Serializers;
using Orleans.Serialization.WireProtocol;

namespace Orleans.Silo.Configuration.Serialization.Codecs;

public class ZonedDateTimeCodec : IGeneralizedCodec, IGeneralizedCopier
{
    public void WriteField<TBufferWriter>(ref Writer<TBufferWriter> writer, uint fieldIdDelta, Type expectedType, object value) where TBufferWriter : IBufferWriter<byte>
    {
        if (typeof(ZonedDateTime) != expectedType)
        {
            throw new Exception("err");
        }
        
        var zonedDateTime = (ZonedDateTime)value;
        writer.WriteInt64(zonedDateTime.ToInstant().ToUnixTimeMilliseconds());
        writer.Commit();
    }

    public object ReadValue<TInput>(ref Reader<TInput> reader, Field field)
    {
        var ms = reader.ReadInt64();
        
        return new ZonedDateTime(Instant.FromUnixTimeMilliseconds(ms), DateTimeZone.Utc);
    }

    bool IGeneralizedCodec.IsSupportedType(Type type)
    {
        return typeof(ZonedDateTime) == type;
    }
    
    bool IGeneralizedCopier.IsSupportedType(Type type)
    {
        return typeof(ZonedDateTime) == type;
    }

    public object DeepCopy(object input, CopyContext context)
    {
       if (input.GetType() != typeof(ZonedDateTime))
       {
           throw new InvalidOperationException("Invalid type");
       }

       var inputZdt = (ZonedDateTime)input;
       var instant = inputZdt.ToInstant();
       var zone = inputZdt.Zone;
       var zdt = new ZonedDateTime(instant, zone); 
       return zdt;
    }
}