using JetBrains.Annotations;

namespace Chatty.MinimalApi.Endpoints;

[UsedImplicitly]
public record GetMessagesRequest(int Page, int PageSize);