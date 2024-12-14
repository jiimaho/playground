namespace Chatty.Silo.Primitives;

public class DomainValidationException(IReadOnlyCollection<string> errors) : Exception
{
    public IReadOnlyCollection<string> Errors { get; set; } = errors;
}