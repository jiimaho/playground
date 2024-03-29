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
        var now = timeProvider.GetLocalNow();
        if (now < expireAt)
        {
            _logger.Debug("Reading from cache");
            var storedJson = await connectionMultiplexer.GetDatabase().StringGetAsync("disasters");
            if (storedJson.IsNullOrEmpty)
            {
                return await StoreInCacheAndGet();
            }
            _logger.Debug("Found data in cache");
            var result = JsonSerializer.Deserialize<IEnumerable<DisasterVm>>(storedJson!);
            return result!;
        }

        _logger.Debug(
            "Will invalidate cache since cache expired at {ExpireAt:yyyy-MM-dd hh-mm-ss} and time now is {Now:yyyy-MM-dd hh-mm-ss}",
            expireAt,
            now);
        return await StoreInCacheAndGet();
    }

    private async Task<IEnumerable<DisasterVm>> StoreInCacheAndGet()
    {
        _logger.Debug("Calling {Type} to get disasters", inner.GetType().Name);
        var disasters = (await inner.GetDisasters()).ToList();
        var json = JsonSerializer.Serialize(disasters);
        await connectionMultiplexer.GetDatabase().StringSetAsync("disasters", new RedisValue(json));
        expireAt = timeProvider.GetLocalNow().Add(ExpirationWindow);
        _logger.Debug("Updated cache");
        return disasters;
    }
}