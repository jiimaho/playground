using Chatty.Silo.Features.SensitiveKeywords.Storage;
using Chatty.Silo.Primitives;

namespace Chatty.Silo.Features.SensitiveKeywords;

public static class ChatMessageValidation
{
    public static ChatMessageValidationResult Validate(this ChatMessage message, SensitiveKeywordsOptions options)
    {
        foreach (var keyword in options.Keywords)
        {
            if (message.Message.Contains(keyword, StringComparison.InvariantCultureIgnoreCase))
                return new ChatMessageValidationResult.SensitiveResult(keyword, message);
        }

        return new ChatMessageValidationResult.NotSensitiveResult();
    }
}