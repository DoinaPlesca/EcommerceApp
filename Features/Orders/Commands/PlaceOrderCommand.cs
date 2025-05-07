using MediatR;
using StackExchange.Redis;
using Order = EcommerceApp.Models.Order;

namespace EcommerceApp.Features.Orders.Commands;

public class PlaceOrderCommand : IRequest<Order>
{
    public string ListingId { get; set; }
    public string BuyerId { get; set; }
}