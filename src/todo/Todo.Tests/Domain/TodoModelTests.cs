using Todo.Domain;

namespace Todo.Tests.Domain;

public class TodoModelTests
{
    [Fact]
    public void WhenComparingEqualModels_ShouldReturnTrue()
    {
        var todo1 = new TodoModel("Test");
        var todo2 = new TodoModel("Test");

        Assert.True(todo1 == todo2);
    }
    
    [Fact]
    public void WhenComparingDifferentModels_ShouldReturnFalse()
    {
        var todo1 = new TodoModel("Test");
        var todo2 = new TodoModel("Test2");

        Assert.False(todo1 == todo2);
    }
    
    [Fact]
    public void WhenComparingDifferentModels2_ShouldReturnFalse()
    {
        var todo1 = new TodoModel("Test");
        var todo2 = new TodoModel("Test2");

        Assert.True(todo1 != todo2);
    }
    
    [Fact]
    public void WhenComparingDifferentModels3_ShouldReturnFalse()
    {
        var todo1 = new TodoModel("Test");
        var todo2 = new TodoModel("Test2");

        Assert.False(todo1.Equals(todo2));
    }
}