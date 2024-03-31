using System.Text.Json;
using StackExchange.Redis;

namespace Disasters.Api.Services;

public class CacheDisastersServiceDecorator(
    IDisastersService inner,
    IConnectionMultiplexer connectionMultiplexer,
    ILogger serilog,
    TimeProvider timeProvider)
    : IDisastersService
{
    private readonly ILogger _logger = serilog.ForContext<CacheDisastersServiceDecorator>();

    private static readonly TimeSpan ExpirationWindow = TimeSpan.FromMinutes(1);
    private static DateTimeOffset expireAt = DateTimeOffset.MinValue;

    public async Task<IEnumerable<DisasterVm>> GetDisasters(int page, int pageSize)
    {
        using var _ = Trace.DisastersApi.StartActivity(GetType());
        var now = timeProvider.GetLocalNow();
        if (now < expireAt)
        {
            var storedJson = await StringGet("disasters");
            if (!storedJson.HasValue)
            {
                return await StoreInCacheAndGet(page, pageSize);
            }
            _logger.Debug("Reading from cache");
            var result = JsonSerializer.Deserialize<IEnumerable<DisasterVm>>(storedJson!);
            return result!;
        }

        _logger.Debug(
            "Will invalidate cache since cache expired at {ExpireAt:yyyy-MM-dd hh-mm-ss} and time now is {Now:yyyy-MM-dd hh-mm-ss}",
            expireAt,
            now);
        return await StoreInCacheAndGet(page, pageSize);
    }

    public Task MarkSafe(MarkSafeVm markSafeVm)
    {
        throw new NotImplementedException();
    }

    private static string GetKey(int page, int pageSize) => $"disasters:{page}:{pageSize}";
    private Task<RedisValue> StringGet(string key) => connectionMultiplexer.GetDatabase().StringGetAsync(key);
    
    private async Task StringSet(string key, RedisValue value) => await connectionMultiplexer.GetDatabase().StringSetAsync(key, value);

    private async Task<IEnumerable<DisasterVm>> StoreInCacheAndGet(int page, int pageSize)
    {
        _logger.Debug(nameof(StoreInCacheAndGet));
        var disasters = (await inner.GetDisasters(page, pageSize)).ToList();
        var json = JsonSerializer.Serialize(disasters);
        await StringSet(GetKey(page, pageSize), json);
        expireAt = timeProvider.GetLocalNow().Add(ExpirationWindow);
        return disasters;
    }
}