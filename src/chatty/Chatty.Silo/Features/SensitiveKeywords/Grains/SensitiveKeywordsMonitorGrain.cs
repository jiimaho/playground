using Azure;
using Azure.Data.Tables;
using Chatty.Silo.Configuration;
using Chatty.Silo.Features.SensitiveKeywords.Storage;
using Chatty.Silo.Primitives;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Orleans.Streams;

namespace Chatty.Silo.Features.SensitiveKeywords.Grains;

[ImplicitStreamSubscription("chat")]
[GrainType("SensitiveKeywordsMonitorGrain")]
[UsedImplicitly]
public class SensitiveKeywordsMonitorGrain(
    IOptionsMonitor<SensitiveKeywordsOptions> options,
    [FromKeyedServices(ChattyTableConstants.SensitiveKeywordsTableName)]
    TableServiceClient tableServiceClient) : Grain, ISensitiveKeywordsMonitorGrain 
{
    private TableClient _table;

    public override async Task OnActivateAsync(CancellationToken cancellationToken)
    {
        try
        {
            // Init table
            await tableServiceClient.CreateTableIfNotExistsAsync(
                ChattyTableConstants.SensitiveKeywordsTableName,
                cancellationToken);
            _table = tableServiceClient.GetTableClient(ChattyTableConstants.SensitiveKeywordsTableName);

            var streamProvider = this.GetStreamProvider("default");
            var stream = streamProvider.GetStream<ChatMessage>(StreamId.Create("chat", this.GetPrimaryKeyString()));
            await stream.SubscribeAsync(OnMessage);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private async Task OnMessage(ChatMessage msg, StreamSequenceToken token)
    {
        var result = msg.Validate(options.CurrentValue);
        
        if (result is ChatMessageValidationResult.SensitiveResult sensitiveResult)
        {
            await AddSensitiveKeywordToTable(msg, sensitiveResult.SensitiveKeyword, token);
        }
    }

    private async Task AddSensitiveKeywordToTable(ChatMessage msg, string keyword, StreamSequenceToken token)
    {
        var cts = new CancellationTokenSource();
        var entity = new SensitiveKeywordEntity
        {
            PartitionKey = msg.GetHashCode().ToString(),
            RowKey = msg.ChatRoomId,
            Timestamp = DateTimeOffset.Now,
            ETag = ETag.All,
            Message = msg.Message,
            Username = msg.Username.Value,
            SensitiveKeyword = keyword
        };
        await _table.AddEntityAsync(entity, cts.Token);
    }
}