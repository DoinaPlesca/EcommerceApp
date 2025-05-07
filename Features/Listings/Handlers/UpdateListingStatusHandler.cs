using EcommerceApp.Features.Listings.Commands;
using EcommerceApp.Models;
using EcommerceApp.Services;
using MediatR;
using MongoDB.Driver;

namespace EcommerceApp.Features.Listings.Handlers;

public class UpdateListingStatusHandler : IRequestHandler<UpdateListingStatusCommand, Listing>
{
    private readonly MongoService _mongo;
    private readonly RedisCacheService _cache;

    public UpdateListingStatusHandler(MongoService mongo, RedisCacheService cache)
    {
        _mongo = mongo;
        _cache = cache;
    }

    public async Task<Listing> Handle(UpdateListingStatusCommand request, CancellationToken cancellationToken)
    {
        var listings = _mongo.GetCollection<Listing>("Listings");
        
        var listing = await listings.Find(x => x.Id == request.ListingId).FirstOrDefaultAsync();
        
        if (listing == null)
            throw new Exception("Listing not found.");
        
        var update = Builders<Listing>.Update
            .Set(x => x.Status, request.NewStatus);

        var result = await listings.UpdateOneAsync(
            x => x.Id == request.ListingId,
            update
        );
        
        await _cache.RemoveAsync($"listings:seller:{listing.SellerId}");
        
        var updatedListing = await listings.Find(x => x.Id == request.ListingId).FirstOrDefaultAsync();
        return updatedListing!;

    }
}