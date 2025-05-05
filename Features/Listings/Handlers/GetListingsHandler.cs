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

    public GetListingsHandler(MongoService mongo)
    {
        _mongo = mongo;
    }

    public async Task<List<Listing>> Handle(GetListingsQuery request, CancellationToken cancellationToken)
    {
        var listings = _mongo.GetCollection<Listing>("Listings");

        var filter = Builders<Listing>.Filter.Empty;

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            filter &= Builders<Listing>.Filter.Regex("Title", new MongoDB.Bson.BsonRegularExpression(request.Search, "i"));
        }

        if (!string.IsNullOrWhiteSpace(request.Category))
        {
            filter &= Builders<Listing>.Filter.Eq("Category", request.Category);
        }

        var sortField = request.SortBy?.ToLower() switch
        {
            "price" => Builders<Listing>.Sort.Ascending(x => x.Price),
            "createdat" => Builders<Listing>.Sort.Descending(x => x.CreatedAt),
            _ => Builders<Listing>.Sort.Descending(x => x.CreatedAt) 
        };

        if (request.SortDirection?.ToLower() == "asc")
        {
            sortField = request.SortBy?.ToLower() switch
            {
                "price" => Builders<Listing>.Sort.Ascending(x => x.Price),
                "createdat" => Builders<Listing>.Sort.Ascending(x => x.CreatedAt),
                _ => Builders<Listing>.Sort.Ascending(x => x.CreatedAt)
            };
        }

        return await listings.Find(filter)
            .Sort(sortField)
            .Skip((request.Page - 1) * request.PageSize)
            .Limit(request.PageSize)
            .ToListAsync();
    }
}