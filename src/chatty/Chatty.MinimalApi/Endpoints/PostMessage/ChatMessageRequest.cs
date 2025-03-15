using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace Chatty.MinimalApi.Endpoints.PostMessage;

[UsedImplicitly]
public record ChatMessageRequest([FromBody] string Message);