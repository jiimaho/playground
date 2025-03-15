using Chatty.Silo.Features.Chatroom.Grains;
using Chatty.Silo.Primitives;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Chatty.MinimalApi.Endpoints.PostMessage;

public static partial class EndpointExtensions
{
    public static WebApplication MapPostMessageEndpoint(this WebApplication app)
    {
        app.MapPost(
                "rooms/{id}/messages",
                async (IValidator<ChatMessageRequest> validator,
                    IClusterClient clusterClient,
                    [FromRoute] string id,
                    ChatMessageRequest request) =>
                {
                    var validationResult = await validator.ValidateAsync(request);
                    if (validationResult.IsValid == false)
                    {
                        return Results.BadRequest(validationResult.Errors);
                    }

                    var chatRoom = clusterClient.GetGrain<IChatRoom>(id);
                    var chatMessage = ChatMessage.Create(Username.Create("RestClient"), request.Message, id);
                    await chatRoom.PostMessage(chatMessage);
                    return Results.Ok();
                })
            .WithName("PostMessage")
            .WithOpenApi();
        return app;
    }
}