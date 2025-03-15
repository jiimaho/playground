using FluentValidation;

namespace Chatty.MinimalApi.Endpoints.PostMessage
{
    public class ChatMessageValidator : AbstractValidator<ChatMessageRequest>
    {
        public ChatMessageValidator()
        {
            RuleFor(r => r.Message).NotNull();
        } 
    }
}