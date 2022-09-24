namespace Todo.Domain;

public class GetAllTodosQueryHandler
{
    private readonly ITodoService _todoService;

    public GetAllTodosQueryHandler(ITodoService todoService)
    {
        _todoService = todoService;
    }
    
    public async Task<IEnumerable<TodoModel>> Handle(GetAllTodosQuery query)
    {
        return await _todoService.GetAllTodos();
    }
}