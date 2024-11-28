using Chatty.Silo.Configuration.Serialization;
using Microsoft.Extensions.Configuration;
using Orleans.Serialization;
using Orleans.TestingHost;

namespace Orleans.Silo.Test.Configuration;

public class ChattySiloClientConfigurator : IClientBuilderConfigurator
{
    public void Configure(IConfiguration configuration, IClientBuilder clientBuilder)
    {
        clientBuilder.Services.AddSerializer(serializerBuilder => serializerBuilder.AddApplicationSpecificSerialization());
    }
}