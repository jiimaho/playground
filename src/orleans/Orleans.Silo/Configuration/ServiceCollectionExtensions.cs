using Orleans.Serialization;

namespace Orleans.Silo.Configuration;

// Do not remove even if empty. Likely needed in the future.
public static class ServiceCollectionExtensions
{
   public static IServiceCollection AddCustomSerialization(this IServiceCollection services)
   {
       // services.AddSerializer(sb =>
       // {
       //     sb.AddJsonSerializer(isSupported: _ => true);
       // });

       return services;
   }    
}