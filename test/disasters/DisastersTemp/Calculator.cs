using System.Collections.ObjectModel;

namespace DisastersTemp;

public class Calculator
{
    public Collection<int> Numbers { get; } = new();

    public void Include(int number)
    {
        Numbers.Add(number);
    }

    public int Add() => Numbers.Sum();
}