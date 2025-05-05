using EcommerceApp.Features.Reviews.Queries;
using EcommerceApp.Models;
using EcommerceApp.Services;
using MediatR;
using MongoDB.Driver;

namespace EcommerceApp.Features.Reviews.Handlers;

public class GetReviewsBySellerHandler : IRequestHandler<GetReviewsBySellerQuery, List<Review>>
{
    private readonly MongoService _mongo;

    public GetReviewsBySellerHandler(MongoService mongo)
    {
        _mongo = mongo;
    }

    public async Task<List<Review>> Handle(GetReviewsBySellerQuery request, CancellationToken cancellationToken)
    {
        var reviews = _mongo.GetCollection<Review>("Reviews");
        return await reviews.Find(r => r.SellerId == request.SellerId).ToListAsync();
    }
}