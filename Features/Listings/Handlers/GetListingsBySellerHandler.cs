using EcommerceApp.Features.Listings.Queries;
using EcommerceApp.Models;
using EcommerceApp.Models.Enums;
using EcommerceApp.Services;
using MediatR;
using MongoDB.Driver;

namespace EcommerceApp.Features.Listings.Handlers;

public class GetListingsBySellerHandler : IRequestHandler<GetListingsBySellerQuery, List<Listing>>
{
    private readonly MongoService _mongo;
    private readonly RedisCacheService _cache;

    public GetListingsBySellerHandler(MongoService mongo, RedisCacheService cache)
    {
        _mongo = mongo;
        _cache = cache;
    }

    public async Task<List<Listing>> Handle(GetListingsBySellerQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"listings:seller:{request.SellerId}";

        return await _cache.GetOrSetAsync<List<Listing>>(
            cacheKey,
            async () =>
            {
                var listings = _mongo.GetCollection<Listing>("Listings");
                return await listings
                    .Find(l => l.SellerId == request.SellerId && l.Status == ListingStatus.Available)
                    .ToListAsync();
            },
            TimeSpan.FromMinutes(15)
        ) ?? new List<Listing>();
    }
}