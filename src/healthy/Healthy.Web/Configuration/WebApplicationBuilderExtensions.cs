using Healthy.Web.Services;
using Healthy.Web.Storage;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

namespace Healthy.Web.Configuration;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddMudServices();

        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddScoped<IReportService, ReportService>();

        builder.Services.AddDbContext<DatabaseContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("Postgres");
            options.UseNpgsql(connectionString);
        });
        return builder;
    }
}