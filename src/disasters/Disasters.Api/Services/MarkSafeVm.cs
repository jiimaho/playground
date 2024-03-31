namespace Disasters.Api.Services;

public class MarkSafeVm
{
    public MarkSafeVm(string name)
    {
        Name = name;
    } 
    public string Name { get; }
}