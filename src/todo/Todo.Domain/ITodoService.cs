namespace Todo.Domain;

public interface ITodoService
{
    Task<IEnumerable<TodoModel>> GetAllTodos();
    Task SaveTodos(IEnumerable<TodoModel> todos);
}