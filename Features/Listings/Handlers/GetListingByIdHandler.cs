using System.Text.Json;
using EcommerceApp.Features.Listings.Queries;
using EcommerceApp.Models;
using EcommerceApp.Services;
using MediatR;
using MongoDB.Driver;

namespace EcommerceApp.Features.Listings.Handlers;

public class GetListingByIdHandler : IRequestHandler<GetListingByIdQuery, Listing>
{
    private readonly MongoService _mongo;
    private readonly RedisCacheService _cache;

    public GetListingByIdHandler(MongoService mongo, RedisCacheService cache)
    {
        _mongo = mongo;
        _cache = cache;
    }

    public async Task<Listing?> Handle(GetListingByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"listing:{request.ListingId}";

        return await _cache.GetOrSetAsync<Listing>(
            cacheKey,
            async () =>
            {
                var listings = _mongo.GetCollection<Listing>("Listings");
                var listing = await listings.Find(x => x.Id == request.ListingId).FirstOrDefaultAsync();

                if (listing == null)
                    throw new Exception("Listing not found");

                return listing;
            },
            TimeSpan.FromMinutes(10)
        );
    }
}