using Todo.Domain;

namespace Todo.Infrastructure;

public class TodoService : ITodoService
{
    private readonly IRepository _repository;

    public TodoService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TodoModel>> GetAllTodos()
    {
        var ss = await _repository.Get("todos");

        var models = ss.Select(s => new TodoModel(s.Name));

        return models;
    }

    public async Task SaveTodos(IEnumerable<TodoModel> todos)
    {
        await _repository.Save("todos", todos.Select(t => new EventDto(t.Name)));
    }
}