using System.Collections.Immutable;

namespace Chatty.Silo.Features.Chatroom.Grains;

[GenerateSerializer]
[Alias("PagingResult<T>")]
public class PagingResult<T>
{
    [Id(0)]
    public int Page { get; set; }
    
    [Id(1)]
    public int PageSize { get; set; }
    
    [Id(2)]
    public int Total { get; set; }
    
    [Id(3)]
    public List<T> Items { get; set; }
}