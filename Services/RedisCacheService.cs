using System.Text.Json;
using StackExchange.Redis;

namespace EcommerceApp.Services;

public class RedisCacheService
{
    private readonly IDatabase _db;
    private static readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web);

    public RedisCacheService(IConnectionMultiplexer redis)
    {
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
    
    public async Task RemoveAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }
    
    //  helper to handle cache logic with fallback
    public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> getData, TimeSpan? expiry = null)
    {
        var cached = await GetAsync(key);

        if (!string.IsNullOrEmpty(cached))
        {
            try
            {
                var deserialized = JsonSerializer.Deserialize<T>(cached, _jsonOptions);
                if (deserialized is not null)
                    return deserialized;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Redis deserialization failed for key '{key}': {ex.Message}");
            }
        }

        var data = await getData();
        if (data is not null)
        {
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            await SetAsync(key, json, expiry);
        }

        return data;
    }
}