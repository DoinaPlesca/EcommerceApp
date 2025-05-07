using EcommerceApp.Models;
using MediatR;

namespace EcommerceApp.Features.Reviews.Commands;

public class CreateReviewCommand : IRequest<Review>
{
    public string OrderId { get; set; }
    public string BuyerId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}