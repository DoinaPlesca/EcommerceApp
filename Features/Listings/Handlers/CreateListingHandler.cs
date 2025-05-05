
using EcommerceApp.Features.Listings.Commands;
using EcommerceApp.Models;
using EcommerceApp.Services;
using MediatR;

namespace EcommerceApp.Features.Listings.Handlers;

public class CreateListingHandler : IRequestHandler<CreateListingCommand, string>
{
    private readonly MongoService _mongo;

    public CreateListingHandler(MongoService mongo)
    {
        _mongo = mongo;
    }

    public async Task<string> Handle(CreateListingCommand request, CancellationToken cancellationToken)
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
        return listing.Id;
    }
}
