using FluentValidation;

namespace Orleans.ChatClient;

public class ChatMessageValidator : AbstractValidator<ChatMessageRequest>
{
    public ChatMessageValidator()
    {
        RuleFor(r => r.Message).NotNull();
    } 
}