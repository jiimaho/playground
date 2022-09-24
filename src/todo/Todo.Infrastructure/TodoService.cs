using Todo.Domain;

namespace Todo.Infrastructure;

public class TodoService : ITodoService
{
    private readonly Repository _repository;

    public TodoService(Repository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TodoModel>> GetAllTodos()
    {
        var ss = await _repository.GetEvents("todos");

        var models = ss.Select(s => new TodoModel(s.Name));

        return models;
    }

    public async Task SaveTodos(IEnumerable<TodoModel> todos)
    {
        await _repository.AppendEvents("todos", todos.Select(t => new EventDto(t.Name)));
    }
}