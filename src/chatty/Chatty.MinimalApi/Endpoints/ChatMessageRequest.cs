using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace Chatty.MinimalApi.Endpoints;

[UsedImplicitly]
public record ChatMessageRequest([FromBody] string Message);