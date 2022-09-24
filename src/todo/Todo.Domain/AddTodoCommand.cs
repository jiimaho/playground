namespace Todo.Domain;

public class AddTodoCommand
{
    public string Name { get; }
    
    public AddTodoCommand(string name)
    {
        Name = name;
    }
}