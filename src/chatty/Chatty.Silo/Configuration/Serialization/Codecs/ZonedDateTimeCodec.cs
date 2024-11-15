// using System.Buffers;
// using System.Text;
// using Amazon.Runtime.EventStreams;
// using NodaTime;
// using Orleans.Serialization;
// using Orleans.Serialization.Buffers;
// using Orleans.Serialization.Cloning;
// using Orleans.Serialization.Codecs;
// using Orleans.Serialization.Serializers;
// using Orleans.Serialization.WireProtocol;
//
// namespace Chatty.Silo.Configuration.Serialization.Codecs;
//
// public class ZonedDateTimeCodec : IGeneralizedCodec, IGeneralizedCopier 
// {
//     public void WriteField<TBufferWriter>(
//         ref Writer<TBufferWriter> writer,
//         uint fieldIdDelta,
//         Type expectedType,
//         object value) where TBufferWriter : IBufferWriter<byte>
//     {
//         if (value == null || expectedType != value.GetType())
//         {
//             throw new ArgumentException($"Value is not of type {expectedType} or is null");
//         }
//
//         if (value.GetType() != typeof(ZonedDateTime))
//         {
//             throw new ArgumentException($"Value is not of type {typeof(ZonedDateTime)}");
//         }
//
//         var zonedDateTime = (ZonedDateTime)value;
//         var instant = zonedDateTime.ToInstant();
//         var zoneId = zonedDateTime.Zone.Id;
//         var calendarId = zonedDateTime.Calendar.Id;
//         // var iso8601Bytes = Encoding.UTF8.GetBytes(instant);
//         // var timeZoneBytes = Encoding.UTF8.GetBytes(timeZone);
//         writer.WriteStartObject(fieldIdDelta, expectedType, value.GetType());
//         writer.WriteFieldHeader(fieldIdDelta, expectedType, value.GetType(), WireType.LengthPrefixed);
//         
//         // Write the instant as ticks (Int64) 
//         var ticksBytes = BitConverter.GetBytes(instant.ToUnixTimeTicks());
//         writer.WriteVarInt64(ticksBytes.Length);// Prefix with length
//         writer.Write(ticksBytes);
//         
//         // Write the time zone as a UTF-8 string 
//         var zoneBytes = Encoding.UTF8.GetBytes(zoneId);
//         writer.WriteVarInt64(zoneBytes.Length);// Prefix with length
//         writer.Write(zoneBytes);
//         
//         // Write the calendar as a UTF-8 string
//         var calendarBytes = Encoding.UTF8.GetBytes(calendarId);
//         writer.WriteVarInt64(calendarBytes.Length);// Prefix with length
//         writer.Write(calendarBytes);
//         
//         writer.WriteEndObject();
//         writer.Commit();
//     }
//
//     public object ReadValue<TInput>(ref Reader<TInput> reader, Field field)
//     {
//         var header = reader.ReadFieldHeader();
//         
//         // if (header.FieldType != typeof(ZonedDateTime))
//         // {
//         //     throw new ArgumentException($"Value is not of type {typeof(ZonedDateTime)}");
//         // }
//         // if (header.WireType != WireType.LengthPrefixed)
//         // {
//         //     throw new ArgumentException($"Value is not of wire type {WireType.LengthPrefixed}");
//         // }
//         var bufferWriter = new ArrayBufferWriter<byte>();
//         
//         // Ticks
//         var ticksLength = reader.ReadVarInt64();
//         reader.ReadBytes(ref bufferWriter, (int)ticksLength);
//         
//         // Zone
//         var zoneLength = reader.ReadVarInt64();
//         reader.ReadBytes(ref bufferWriter, (int)zoneLength);
//         
//         // Calendar
//         var calendarLength = reader.ReadVarInt64();
//         reader.ReadBytes(ref bufferWriter, (int)calendarLength);
//         
//         var ticks = BitConverter.ToInt64(bufferWriter.WrittenSpan[..(int)ticksLength]);
//         var zone = Encoding.UTF8.GetString(bufferWriter.WrittenSpan[(int)ticksLength..(int)(ticksLength + zoneLength)]);
//         var calendar = Encoding.UTF8.GetString(bufferWriter.WrittenSpan[(int)(ticksLength + zoneLength)..(int)(ticksLength + zoneLength + calendarLength)]);
//
//         return new ZonedDateTime(
//             Instant.FromUnixTimeTicks(ticks),
//             DateTimeZoneProviders.Tzdb[zone],
//             CalendarSystem.ForId(calendar));
//     }
//
//     bool IGeneralizedCodec.IsSupportedType(Type type)
//     {
//         return typeof(ZonedDateTime) == type;
//     }
//
//     bool IGeneralizedCopier.IsSupportedType(Type type)
//     {
//         return typeof(ZonedDateTime) == type;
//     }
//
//     public object DeepCopy(object input, CopyContext context)
//     {
//         if (input.GetType() != typeof(ZonedDateTime))
//         {
//             throw new InvalidOperationException("Invalid type");
//         }
//
//         var inputZdt = (ZonedDateTime)input;
//         var instant = inputZdt.ToInstant();
//         var zone = inputZdt.Zone;
//         var zdt = new ZonedDateTime(instant, zone);
//         return zdt;
//     }
// }