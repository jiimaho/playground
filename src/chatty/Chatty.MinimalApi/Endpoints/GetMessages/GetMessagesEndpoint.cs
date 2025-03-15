using Chatty.Silo.Features.Chatroom.Grains;
using FluentValidation;

namespace Chatty.MinimalApi.Endpoints.GetMessages;

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
                        request.Page,
                        request.PageSize,
                        pagingResult.NumberOfPages,
                        pagingResult.HasNextPage,
                        pagingResult.Total,
                        Messages = pagingResult.Items.Select(m => new { m.Username, m.Message, m.ChatRoomId, m.Timestamp })
                    });
                })
            .WithName("GetMessages")
            .WithOpenApi();
        return app;
    }
}