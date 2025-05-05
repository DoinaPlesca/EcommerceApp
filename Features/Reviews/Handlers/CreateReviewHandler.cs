using EcommerceApp.Features.Reviews.Commands;
using EcommerceApp.Models;
using EcommerceApp.Services;
using MediatR;
using MongoDB.Driver;

namespace EcommerceApp.Features.Reviews.Handlers;

public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, string>
{
    private readonly MongoService _mongo;
    private readonly RedisCacheService _cache;

    public CreateReviewHandler(MongoService mongo, RedisCacheService cache)
    {
        _mongo = mongo;
        _cache = cache;
    }

    public async Task<string> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var orders = _mongo.GetCollection<Order>("Orders");
        var reviews = _mongo.GetCollection<Review>("Reviews");

        var order = await orders.Find(x => x.Id == request.OrderId && x.BuyerId == request.BuyerId).FirstOrDefaultAsync();
        if (order == null)
            throw new Exception("Invalid order or not authorized.");
        

        var existingReview = await reviews.Find(x => x.OrderId == request.OrderId).FirstOrDefaultAsync();
        if (existingReview != null)
            throw new Exception("Review already exists for this order.");

        var review = new Review
        {
            OrderId = request.OrderId,
            SellerId = order.SellerId,
            BuyerId = request.BuyerId,
            Rating = request.Rating,
            Comment = request.Comment
        };

        await reviews.InsertOneAsync(review);
        
        // invalidate cache for seller review
        var cacheKey = $"reviews:seller:{order.SellerId}";
        await _cache.RemoveAsync(cacheKey);
        
        return review.Id;
    }
}
