using Chatty.Silo.Primitives;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Playwright;
using NodaTime;
using ChatMessage = Chatty.Silo.Primitives.ChatMessage;
using ChatMessageComponent = Chatty.Web.Components.Pages.ChatMessage;

namespace Chatty.Web.Endpoints;

public static class Report
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IEndpointRouteBuilder AddReportRoute(this IEndpointRouteBuilder routes)
    {
        routes.Map("report", async context =>
        {
            var sp = context.RequestServices;
            var lf = sp.GetRequiredService<ILoggerFactory>();

            var time = DateTimeOffset.UtcNow;
            var htmlRenderer = new HtmlRenderer(sp, lf);
            await htmlRenderer.Dispatcher.InvokeAsync(async () =>
            {
                var dictionary = new Dictionary<string, object?>
                    { { "Message", new  ChatMessage { 
                        Username = new Username("Alice"), 
                        Message = "Hello, World!", 
                        Timestamp = time, 
                        ChatRoomId = "1", 
                        TimeStampNoda = ZonedDateTime.FromDateTimeOffset(time) 
                    }} };

                var parameters = ParameterView.FromDictionary(dictionary);
                var html = await htmlRenderer.RenderComponentAsync<ChatMessageComponent>(parameters);
                Console.WriteLine(html.ToHtmlString());
                var content = html.ToHtmlString();

                return CreatePdf(content);
            });
        });
        return routes;
    }

    private static async Task CreatePdf(string content)
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser =
            await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });

        var page = await browser.NewPageAsync();
        await page.SetContentAsync(content);
        await page.PdfAsync(new PagePdfOptions { Format = "A4", Path = "./report.pdf" });
        await page.CloseAsync();
    }
}