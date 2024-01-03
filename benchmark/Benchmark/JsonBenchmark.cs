using System.Text;
using System.Text.Json;
using BenchmarkDotNet.Attributes;

namespace Benchmark;

[MemoryDiagnoser]
public class JsonBenchmark
{
    [Benchmark]
    public async Task ReadJsonPerformantUsingStream()
    {
        await using var fileStream = File.OpenRead("imdb_movies.json");
        
        var j2 = await JsonSerializer.DeserializeAsync<object>(fileStream);
    }
    
    [Benchmark]
    public async Task ReadJsonSlowUsingHeavyMemory()
    {
        var json = await File.ReadAllTextAsync("imdb_movies.json");

        using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var j2 = await JsonSerializer.DeserializeAsync<object>(memoryStream);
    }
}