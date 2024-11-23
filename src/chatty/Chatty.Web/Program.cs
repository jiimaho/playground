using Blazored.LocalStorage;
using Chatty.Silo.Configuration.Serialization;
using Chatty.Web.Components;
using Chatty.Web.Endpoints;
using Orleans.Configuration;
using Orleans.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddBlazoredLocalStorage();

builder.Host.UseOrleansClient((ctx, clientBuilder) =>
{
    clientBuilder.UseDynamoDBClustering(options => { options.Service = "eu-west-1"; });
    clientBuilder.Configure<ClusterOptions>(options =>
    {
        options.ClusterId = "blazor-cluster";
        options.ServiceId = "blazor-service";
    });
    clientBuilder.Services.AddSerializer(sb => sb.AddApplicationSpecificSerialization());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();
app.UseEndpoints(erb => erb.AddReportRoute());

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Chatty.Web.Client._Imports).Assembly);

app.Run();