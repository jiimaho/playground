using Azure;
using Azure.Data.Tables;

namespace Chatty.Silo.Features.SensitiveKeywords.Storage;

public class SensitiveKeywordEntity : ITableEntity
{
    // ITableEntity
    public required string PartitionKey { get; set; }
    public required string RowKey { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    
    // Custom
    public required string Username { get; set; }
    public required string Message { get; set; }
    public required string SensitiveKeyword { get; set; }
}