namespace Todo.Domain;

public class AddTodoCommandHandler
{
    private readonly ITodoService _todoService;

    public AddTodoCommandHandler(ITodoService todoService)
    {
        _todoService = todoService;
    }
    
    public async Task Handle(AddTodoCommand command)
    {
        var todo = new TodoModel(command.Name);
        await _todoService.SaveTodos(new []{ todo });
    }
}