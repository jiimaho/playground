namespace Todo.Infrastructure;

public class EventDto
{
    public string Name { get; set; }
    
    public EventDto(string name)
    {
        Name = name;
    }
}