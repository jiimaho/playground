using Chatty.Silo.Features.Chatroom.Grains;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Chatty.MinimalApi.Endpoints;

public static partial class EndpointExtensions
{
    public static WebApplication MapGetMessagesEndpoint(this WebApplication app)
    {
        app.MapGet(
                "rooms/{id}/messages",
                async (IClusterClient clusterClient,
                    string id,
                    [AsParameters] GetMessagesRequest request,
                    IValidator<GetMessagesRequest> validator) =>
                {
                    var result = await validator.ValidateAsync(request);
                    if (!result.IsValid)
                    {
                        return Results.BadRequest(result.Errors);
                    }

                    var chatRoom = clusterClient.GetGrain<IChatRoom>(id);
                    var src = new GrainCancellationTokenSource();
                    var pagingResult= await chatRoom.GetHistoryPaging(request.Page, request.PageSize, src.Token);
                    return Results.Ok(new 
                    {
                        Page = request.Page,
                        PageSize = request.PageSize,
                        Total = pagingResult.Total,
                        Messages = pagingResult.Items
                    });
                })
            .WithName("GetMessages")
            .WithOpenApi();
        return app;
    }
}