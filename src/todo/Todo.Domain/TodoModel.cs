namespace Todo.Domain;

public class TodoModel : ValueObject
{
    public string Name { get; }

    public TodoModel(string name)
    {
        Name = name;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}