using StackExchange.Redis;

namespace EcommerceApp.Services;

public class RedisCacheService
{
    private readonly IDatabase _db;

    public RedisCacheService(IConfiguration config)
    {
        var redis = ConnectionMultiplexer.Connect(config["Redis:Host"]);
        _db = redis.GetDatabase();
    }

    public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
    {
        await _db.StringSetAsync(key, value, expiry);
    }

    public async Task<string?> GetAsync(string key)
    {
        return await _db.StringGetAsync(key);
    }
}