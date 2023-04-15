namespace Todo.Infrastructure.EntityFramework;

public class EntityFrameworkRepository : IRepository
{
    private readonly TodoDb _todoDb;

    public EntityFrameworkRepository(TodoDb todoDb)
    {
        _todoDb = todoDb;
    }
    
    public Task<IEnumerable<EventDto>> Get(string stream)
    {
        throw new NotImplementedException();
    }

    public Task Save(string stream, IEnumerable<EventDto> events)
    {
        throw new NotImplementedException();
    }
}