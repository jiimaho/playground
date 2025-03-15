using JetBrains.Annotations;

namespace Chatty.MinimalApi.Endpoints.GetMessages;

[UsedImplicitly]
public record GetMessagesRequest(int Page, int PageSize);