using EcommerceApp.Features.Orders.Commands;
using EcommerceApp.Models;
using EcommerceApp.Models.Enums;
using EcommerceApp.Services;
using MediatR;
using MongoDB.Driver;

namespace EcommerceApp.Features.Orders.Handlers;

public class PlaceOrderHandler : IRequestHandler<PlaceOrderCommand, Order>
{
    private readonly MongoService _mongo;

    public PlaceOrderHandler(MongoService mongo)
    {
        _mongo = mongo;
    }

    public async Task<Order> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
    {
        var listings = _mongo.GetCollection<Listing>("Listings");
        var orders = _mongo.GetCollection<Order>("Orders");

        var listing = await listings.Find(x => x.Id == request.ListingId).FirstOrDefaultAsync();
        if (listing == null)
            throw new Exception("Listing not found.");

        var existingOrder = await orders.Find(o => o.ListingId == listing.Id).FirstOrDefaultAsync();
        if (existingOrder != null)
            throw new Exception("Item already ordered.");

        var order = new Order
        {
            ListingId = listing.Id,
            SellerId = listing.SellerId,
            BuyerId = request.BuyerId,
            TotalPrice = listing.Price,
            Status = OrderStatus.Pending 
        };

        await orders.InsertOneAsync(order);
        return order;
    }
}