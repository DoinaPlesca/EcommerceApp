using System.Text.Json;
using EcommerceApp.Features.Reviews.Queries;
using EcommerceApp.Models;
using EcommerceApp.Services;
using MediatR;
using MongoDB.Driver;

namespace EcommerceApp.Features.Reviews.Handlers;

public class GetReviewsBySellerHandler : IRequestHandler<GetReviewsBySellerQuery, List<Review>>
{
    private readonly MongoService _mongo;
    private readonly RedisCacheService _cache;

    public GetReviewsBySellerHandler(MongoService mongo, RedisCacheService cache)
    {
        _mongo = mongo;
        _cache = cache;
    }

    public async Task<List<Review>> Handle(GetReviewsBySellerQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"reviews:seller:{request.SellerId}";
        
        return await _cache.GetOrSetAsync<List<Review>>(
            cacheKey,
            async () =>
            {
                var reviews = _mongo.GetCollection<Review>("Reviews");
                var result = await reviews.Find(r => r.SellerId == request.SellerId).ToListAsync();
                return result;
            },
            TimeSpan.FromMinutes(15)
        ) ?? new List<Review>(); // fallback if cache or DB somehow returns null
    }
}