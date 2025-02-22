using Chatty.Silo.Primitives;
using Dunet;

namespace Chatty.Silo.Features.SensitiveKeywords;

[Union]
public partial record ChatMessageValidationResult
{
    public partial record SensitiveResult(string SensitiveKeyword, ChatMessage ChatMessage);

    public partial record NotSensitiveResult;
}