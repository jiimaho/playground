namespace Chatty.Silo.Features;

[GenerateSerializer]
[Alias("Chatty.Silo.Features.PagingResult`1")]
public class PagingResult<T>
{
    [Id(0)]
    public int Page { get; set; }
    
    [Id(1)]
    public int PageSize { get; set; }
    
    [Id(2)]
    public bool HasNextPage { get; set; }
    
    [Id(3)]
    public int NumberOfPages { get; set; }
    
    [Id(4)]
    public int Total { get; set; }

    [Id(5)]
    public List<T> Items { get; set; } = [];
}