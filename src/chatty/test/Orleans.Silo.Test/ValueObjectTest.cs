
using Chatty.Silo.Primitives;

namespace Orleans.Silo.Test;

public class ValueObjectTest
{
    [Fact]
    public void Username_Is_Equal()
    {
        var username1 = Username.Create("Jim");
        var username2 = Username.Create("Jim");
        
        Assert.Equal(username1, username2);
    }
    
    [Fact]
    public void Username_Is_Not_Equal()
    {
        var username1 = Username.Create("Jim");
        var username2 = Username.Create("Bob");
        
        Assert.NotEqual(username1, username2);
    }
    
    [Fact]
    public void Username_Throws_Exception_When_Null()
    {
        Assert.Throws<ArgumentNullException>(() => Username.Create(null));
    }
    
    [Fact]
    public void Username_Is_Not_Equal_When_Different_Types()
    {
        var username = Username.Create("Jim");
        
        Assert.NotEqual(username, new object());
    }
    
    [Fact]
    public void Username_Is_Not_Equal_When_Other_Is_Null()
    {
        Username username = Username.Create("Jim");
        Username username2 = null!;
         
        Assert.NotEqual(username2, username);
    }
    
    [Fact]
    public void Cannot_Add_Multiple_Identical_In_Dictionary()
    {
        var dictionary = new Dictionary<Username, DateTimeOffset>();
        var username1 = Username.Create("Jim");
        dictionary[username1] = DateTimeOffset.UtcNow;
        dictionary[username1] = DateTimeOffset.UtcNow;
        
        Assert.Single(dictionary);
    }
    
    [Fact]
    public void Cannot_Add_Multiple_Same_In_Dictionary()
    {
        var dictionary = new Dictionary<Username, DateTimeOffset>();
        var username1 = Username.Create("Jim");
        var username2 = Username.Create("Jim");
        dictionary[username1] = DateTimeOffset.UtcNow;
        dictionary[username2] = DateTimeOffset.UtcNow;
        
        Assert.Single(dictionary);
    }
    
    [Fact]
    public void Tst()
    {
        var username1 = Username.Create("Jim");
        var username2 = Username.Create("Jim");

        var equals = username1.Equals(username2);
        var hashCodeEquals = username1.GetHashCode() == username2.GetHashCode();
        
        Assert.True(equals);
        Assert.True(hashCodeEquals);  
    }
}