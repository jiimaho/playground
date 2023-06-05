namespace Disasters.Api.Test;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        string p1 = "Jim";
        string p2 = "Jim ".Trim();

        Assert.Same(p1, p2);
    }
}