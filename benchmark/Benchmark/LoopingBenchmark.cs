using BenchmarkDotNet.Attributes;

namespace Benchmark;

public class LoopingBenchmark
{
    private List<int> list;
    private IReadOnlyList<int> readOnlyList;

    [GlobalSetup]
    public void Setup()
    {
        list = Enumerable.Range(0, 1_000_000).ToList();
        readOnlyList = list.AsReadOnly();
    }

    [Benchmark]
    public void SummarizeByLoopingThroughList()
    {
        var sum = 0;
        foreach (var item in list)
        {
            sum += item;
        }
    }
    
    [Benchmark]
    public void SummarizeByLoopingThroughReadOnlyList()
    {
        var sum = 0;
        foreach (var item in readOnlyList)
        {
            sum += item;
        }
    }
}