using EcommerceApp.Features.Listings.Commands;
using EcommerceApp.Models;
using EcommerceApp.Services;
using MediatR;
using MongoDB.Driver;

namespace EcommerceApp.Features.Listings.Handlers;

public class UpdateListingStatusHandler : IRequestHandler<UpdateListingStatusCommand, bool>
{
    private readonly MongoService _mongo;

    public UpdateListingStatusHandler(MongoService mongo)
    {
        _mongo = mongo;
    }

    public async Task<bool> Handle(UpdateListingStatusCommand request, CancellationToken cancellationToken)
    {
        var listings = _mongo.GetCollection<Listing>("Listings");

        var update = Builders<Listing>.Update
            .Set(x => x.Status, request.NewStatus);

        var result = await listings.UpdateOneAsync(
            x => x.Id == request.ListingId,
            update
        );

        return result.ModifiedCount > 0;
    }
}