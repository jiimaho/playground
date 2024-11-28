using Chatty.Silo.Configuration;
using Chatty.Silo.Configuration.Serialization;
using Orleans.Serialization;
using Orleans.TestingHost;

namespace Orleans.Silo.Test.Configuration;

public class ChattySiloConfigurator : ISiloConfigurator
{
    public void Configure(ISiloBuilder siloBuilder)
    {
        siloBuilder.AddMemoryGrainStorage(GrainStorage.ChatRoomStore);
        siloBuilder.Services.AddSerializer(serializerBuilder => serializerBuilder.AddApplicationSpecificSerialization());
    }
}