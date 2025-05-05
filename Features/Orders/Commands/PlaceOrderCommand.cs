using MediatR;

namespace EcommerceApp.Features.Orders.Commands;

public class PlaceOrderCommand : IRequest<string>
{
    public string ListingId { get; set; }
    public string BuyerId { get; set; }
}