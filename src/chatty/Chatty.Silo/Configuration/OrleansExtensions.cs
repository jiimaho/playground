using Chatty.Silo.Configuration.Serialization;
using JetBrains.Annotations;
using Orleans.Configuration;
using Orleans.Serialization;

namespace Chatty.Silo.Configuration;

public static class OrleansExtensions
{
    [UsedImplicitly]
    // public static ISiloBuilder UseChattyOrleans(this ISiloBuilder siloBuilder, HostBuilderContext context)
    public static ISiloBuilder UseChattyOrleans(this ISiloBuilder siloBuilder, IConfiguration configuration, IWebHostEnvironment environment)
    {
        // Silo cleanup
        siloBuilder.Services.Configure<ClusterMembershipOptions>(options =>
        {
            options.DefunctSiloCleanupPeriod = TimeSpan.FromMinutes(1);
            options.DefunctSiloExpiration = TimeSpan.FromMinutes(1);
        });

        siloBuilder.Services.AddSerializer(sb => sb.AddApplicationSpecificSerialization());

        siloBuilder.AddMemoryStreams("default");
        siloBuilder.AddMemoryGrainStorage("default");
        
        siloBuilder.Services.AddLogging();

        return siloBuilder;
    }
}