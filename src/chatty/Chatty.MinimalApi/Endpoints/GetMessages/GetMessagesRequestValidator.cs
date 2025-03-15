using FluentValidation;

namespace Chatty.MinimalApi.Endpoints.GetMessages;

public class GetMessagesRequestValidator : AbstractValidator<GetMessagesRequest>
{
    public GetMessagesRequestValidator()
    {
        RuleFor(r => r.Page).GreaterThanOrEqualTo(1);
        RuleFor(r => r.PageSize).GreaterThan(0).LessThanOrEqualTo(10);
    }
}