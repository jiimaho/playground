using Chatty.Silo.Configuration.Serialization.Codecs;
using NodaTime;
using Orleans.Serialization;
using Orleans.Serialization.Cloning;
using Orleans.Serialization.Codecs;

namespace Chatty.Silo.Configuration.Serialization;

// Do not remove even if empty. Likely needed in the future.
public static class SerializerBuilderExtensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static ISerializerBuilder AddApplicationSpecificSerialization(this ISerializerBuilder builder)
    {
        var services = builder.Services;  
        services.AddSingleton<ZonedDateTimeCodec>();
        // services.AddSingleton<IGeneralizedCodec, ZonedDateTimeCodec>();
        services.AddSingleton<IFieldCodec<ZonedDateTime>, ZonedDateTimeCodec>();
        services.AddSingleton<IGeneralizedCopier, ZonedDateTimeCodec>();

       return builder;
   }    
}