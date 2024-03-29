using System.Diagnostics;
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

    public async Task<IEnumerable<DisasterVm>> GetDisasters()
    {
        using var _ = Tracing.StartCustomActivity(GetType());
        var now = timeProvider.GetLocalNow();
        if (now < expireAt)
        {
            var storedJson = await StringGet("disasters");
            if (!storedJson.HasValue)
            {
                return await StoreInCacheAndGet();
            }
            _logger.Debug("Reading from cache");
            var result = JsonSerializer.Deserialize<IEnumerable<DisasterVm>>(storedJson!);
            return result!;
        }

        _logger.Debug(
            "Will invalidate cache since cache expired at {ExpireAt:yyyy-MM-dd hh-mm-ss} and time now is {Now:yyyy-MM-dd hh-mm-ss}",
            expireAt,
            now);
        return await StoreInCacheAndGet();
    }

    private Task<RedisValue> StringGet(string key) => connectionMultiplexer.GetDatabase().StringGetAsync(key);
    
    private async Task StringSet(string key, RedisValue value) => await connectionMultiplexer.GetDatabase().StringSetAsync(key, value);

    private async Task<IEnumerable<DisasterVm>> StoreInCacheAndGet()
    {
        _logger.Debug(nameof(StoreInCacheAndGet));
        var disasters = (await inner.GetDisasters()).ToList();
        var json = JsonSerializer.Serialize(disasters);
        await StringSet("disasters", json);
        expireAt = timeProvider.GetLocalNow().Add(ExpirationWindow);
        return disasters;
    }
}