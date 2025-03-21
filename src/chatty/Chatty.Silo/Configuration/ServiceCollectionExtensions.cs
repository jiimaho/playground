using Chatty.Silo.Features.SensitiveKeywords.Storage;

namespace Chatty.Silo.Configuration;

public static class ServiceCollectionExtensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.AddKeyedAzureTableClient("sensitive");
        builder.AddKeyedAzureTableClient("clustering");
        builder.AddKeyedAzureBlobClient("grain-state");
        
        builder.Services.Configure<SensitiveKeywordsOptions>(
            builder.Configuration.GetRequiredSection(SensitiveKeywordsOptions.SectionName));

        return builder;
    }
}