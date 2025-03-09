using FluentValidation;

namespace Chatty.MinimalApi.Endpoints;

public class GetMessagesRequestValidator : AbstractValidator<GetMessagesRequest>
{
    public GetMessagesRequestValidator()
    {
        RuleFor(r => r.Page).GreaterThanOrEqualTo(0);
        RuleFor(r => r.PageSize).GreaterThan(0).LessThanOrEqualTo(10);
    }
}