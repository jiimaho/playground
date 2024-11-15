using System.Buffers;
using System.Text;
using NodaTime;
using Orleans.Serialization.Buffers;
using Orleans.Serialization.Cloning;
using Orleans.Serialization.Codecs;
using Orleans.Serialization.WireProtocol;

namespace Orleans.Silo.Configuration.Serialization.Codecs;

public class ZonedDateTimeCodec : IGeneralizedCopier, IFieldCodec<ZonedDateTime>
{
    ZonedDateTime IFieldCodec<ZonedDateTime>.ReadValue<TInput>(ref Reader<TInput> reader, Field field)
    {
        var bufferWriter = new ArrayBufferWriter<byte>();

        // Start header
        var header = reader.ReadFieldHeader();
        
        // Ticks
        var ticksHeader = reader.ReadFieldHeader();
        var ticksLength = reader.ReadVarInt64();
        reader.ReadBytes(ref bufferWriter, (int)ticksLength);

        // Zone
        var zoneHeader = reader.ReadFieldHeader();
        var zoneLength = reader.ReadVarInt64();
        reader.ReadBytes(ref bufferWriter, (int)zoneLength);

        // Calendar
        var calendarHeader = reader.ReadFieldHeader();
        var calendarLength = reader.ReadVarInt64();
        reader.ReadBytes(ref bufferWriter, (int)calendarLength);

        // End header
        var end = reader.ReadFieldHeader();

        var ticks = BitConverter.ToInt64(bufferWriter.WrittenSpan[..(int)ticksLength]);
        var zone = Encoding.UTF8.GetString(bufferWriter.WrittenSpan[(int)ticksLength..(int)(ticksLength + zoneLength)]);
        var calendar =
            Encoding.UTF8.GetString(
                bufferWriter.WrittenSpan[
                    (int)(ticksLength + zoneLength)..(int)(ticksLength + zoneLength + calendarLength)]);

        return new ZonedDateTime(
            Instant.FromUnixTimeTicks(ticks),
            DateTimeZoneProviders.Tzdb[zone],
            CalendarSystem.ForId(calendar));
    }

    public void WriteField<TBufferWriter>(
        ref Writer<TBufferWriter> writer,
        uint fieldIdDelta,
        Type expectedType,
        ZonedDateTime value) where TBufferWriter : IBufferWriter<byte>
    {
        if (value == null || expectedType != value.GetType())
        {
            throw new ArgumentException($"Value is not of type {expectedType} or is null");
        }

        if (value.GetType() != typeof(ZonedDateTime))
        {
            throw new ArgumentException($"Value is not of type {typeof(ZonedDateTime)}");
        }

        var zonedDateTime = value;
        var instant = zonedDateTime.ToInstant();
        var zoneId = zonedDateTime.Zone.Id;
        var calendarId = zonedDateTime.Calendar.Id;
        writer.WriteStartObject(fieldIdDelta, expectedType, value.GetType());
        writer.WriteFieldHeader(fieldIdDelta, expectedType, value.GetType(), WireType.TagDelimited);

        // Write the instant as ticks (Int64) 
        var ticksBytes = BitConverter.GetBytes(instant.ToUnixTimeTicks());
        writer.WriteFieldHeaderExpected(0, WireType.LengthPrefixed);
        writer.WriteVarInt64(ticksBytes.Length); // Prefix with length
        writer.Write(ticksBytes);

        // Write the time zone as a UTF-8 string 
        var zoneBytes = Encoding.UTF8.GetBytes(zoneId);
        writer.WriteFieldHeaderExpected(1, WireType.LengthPrefixed);
        writer.WriteVarInt64(zoneBytes.Length); // Prefix with length
        writer.Write(zoneBytes);

        // Write the calendar as a UTF-8 string
        var calendarBytes = Encoding.UTF8.GetBytes(calendarId);
        writer.WriteFieldHeaderExpected(2, WireType.LengthPrefixed);
        writer.WriteVarInt64(calendarBytes.Length); // Prefix with length
        writer.Write(calendarBytes);

        writer.WriteEndObject();
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