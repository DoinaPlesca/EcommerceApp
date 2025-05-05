using EcommerceApp.Features.Orders.Commands;
using EcommerceApp.Models;
using EcommerceApp.Services;
using MediatR;
using MongoDB.Driver;

namespace EcommerceApp.Features.Orders.Handlers;

public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
{
    private readonly MongoService _mongo;

    public UpdateOrderStatusHandler(MongoService mongo)
    {
        _mongo = mongo;
    }

    public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var orders = _mongo.GetCollection<Order>("Orders");

        var update = Builders<Order>.Update.Set(o => o.Status, request.NewStatus);
        var result = await orders.UpdateOneAsync(
            o => o.Id == request.OrderId,
            update
        );

        return result.ModifiedCount > 0;
    }
}