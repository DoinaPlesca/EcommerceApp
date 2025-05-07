using EcommerceApp.Models;
using EcommerceApp.Models.Enums;
using MediatR;

namespace EcommerceApp.Features.Orders.Commands;

public class UpdateOrderStatusCommand : IRequest<Order>
{
    public string OrderId { get; set; }
    
    public OrderStatus NewStatus { get; set; }
}