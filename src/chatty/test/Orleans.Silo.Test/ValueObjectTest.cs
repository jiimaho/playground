
using Chatty.Silo.Primitives;

namespace Orleans.Silo.Test;

public class ValueObjectTest
{
    [Fact]
    public void Username_Is_Equal()
    {
        var username1 = new Username("Jim");
        var username2 = new Username("Jim");
        
        Assert.Equal(username1, username2);
    }
    
    [Fact]
    public void Username_Is_Not_Equal()
    {
        var username1 = new Username("Jim");
        var username2 = new Username("Bob");
        
        Assert.NotEqual(username1, username2);
    }
    
    [Fact]
    public void Username_Throws_Exception_When_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new Username(null!));
    }
    
    [Fact]
    public void Username_Is_Not_Equal_When_Different_Types()
    {
        var username = new Username("Jim");
        
        Assert.NotEqual(username, new object());
    }
    
    [Fact]
    public void Username_Is_Not_Equal_When_Other_Is_Null()
    {
        Username username = new Username("Jim");
        Username username2 = null!;
         
        Assert.NotEqual(username2, username);
    }
    
    [Fact]
    public void Cannot_Add_Multiple_Identical_In_Dictionary()
    {
        var dictionary = new Dictionary<Username, DateTimeOffset>();
        var username1 = new Username("Jim");
        dictionary[username1] = DateTimeOffset.UtcNow;
        dictionary[username1] = DateTimeOffset.UtcNow;
        
        Assert.Single(dictionary);
    }
    
    [Fact]
    public void Cannot_Add_Multiple_Same_In_Dictionary()
    {
        var dictionary = new Dictionary<Username, DateTimeOffset>();
        var username1 = new Username("Jim");
        var username2 = new Username("Jim");
        dictionary[username1] = DateTimeOffset.UtcNow;
        dictionary[username2] = DateTimeOffset.UtcNow;
        
        Assert.Single(dictionary);
    }
    
    [Fact]
    public void Tst()
    {
        var username1 = new Username("Jim");
        var username2 = new Username("Jim");

        var equals = username1.Equals(username2);
        var hashCodeEquals = username1.GetHashCode() == username2.GetHashCode();
        
        Assert.True(equals);
        Assert.True(hashCodeEquals);  
    }
}