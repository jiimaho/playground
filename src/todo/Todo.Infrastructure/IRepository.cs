namespace Todo.Infrastructure;

public interface IRepository
{
    Task<IEnumerable<EventDto>> Get(string stream);
    
    Task Save(string stream, IEnumerable<EventDto> events);
}