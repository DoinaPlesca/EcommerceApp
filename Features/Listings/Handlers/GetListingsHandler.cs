using System.Text.Json;
using EcommerceApp.Features.Listings.Querie;
using EcommerceApp.Models;
using EcommerceApp.Services;
using MediatR;
using MongoDB.Driver;

namespace EcommerceApp.Features.Listings.Handlers;

public class GetListingsHandler : IRequestHandler<GetListingsQuery, List<Listing>>
{
    private readonly MongoService _mongo;
    private readonly RedisCacheService _cache;

    public GetListingsHandler(MongoService mongo, RedisCacheService cache)
    {
        _mongo = mongo;
        _cache = cache;
    }

    public async Task<List<Listing>> Handle(GetListingsQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"listings:{request.Search}:{request.Category}";
        var cached = await _cache.GetAsync(cacheKey);
        if (cached != null)
            return JsonSerializer.Deserialize<List<Listing>>(cached);

        var filter = Builders<Listing>.Filter.Empty;

        if (!string.IsNullOrEmpty(request.Search))
            filter &= Builders<Listing>.Filter.Regex("Title", new MongoDB.Bson.BsonRegularExpression(request.Search, "i"));

        if (!string.IsNullOrEmpty(request.Category))
            filter &= Builders<Listing>.Filter.Eq("Category", request.Category);

        var collection = _mongo.GetCollection<Listing>("Listings");
        var listings = await collection.Find(filter).ToListAsync();

        await _cache.SetAsync(cacheKey, JsonSerializer.Serialize(listings), TimeSpan.FromMinutes(5));
        return listings;
    }
}
