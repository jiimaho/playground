using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace Orleans.ChatClient;

[UsedImplicitly]
public record ChatMessageRequest([FromBody] string Message);