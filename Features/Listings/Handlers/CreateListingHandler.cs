
using EcommerceApp.Features.Listings.Commands;
using EcommerceApp.Models;
using EcommerceApp.Services;
using MediatR;

namespace EcommerceApp.Features.Listings.Handlers;

public class CreateListingHandler : IRequestHandler<CreateListingCommand, Listing>
{
    private readonly MongoService _mongo;
    private readonly RedisCacheService _cache;

    public CreateListingHandler(MongoService mongo, RedisCacheService cache)
    {
        _mongo = mongo;
        _cache = cache;
    }

    public async Task<Listing> Handle(CreateListingCommand request, CancellationToken cancellationToken)
    {
        var listing = new Listing
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            SellerId = request.SellerId,
            Category = request.Category,
            Condition = request.Condition,
            ImageUrls = request.ImageUrls
        };

        var collection = _mongo.GetCollection<Listing>("Listings");
        await collection.InsertOneAsync(listing);
        
        //  invalidate seller listing cache
        var cacheKey = $"listings:seller:{request.SellerId}";
        await _cache.RemoveAsync(cacheKey);
        
        return listing;
    }
}
