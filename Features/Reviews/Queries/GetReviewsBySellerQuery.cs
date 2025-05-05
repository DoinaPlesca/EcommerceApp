using EcommerceApp.Models;
using MediatR;

namespace EcommerceApp.Features.Reviews.Queries;

public class GetReviewsBySellerQuery : IRequest<List<Review>>
{
    public string SellerId { get; }

    public GetReviewsBySellerQuery(string sellerId)
    {
        SellerId = sellerId ?? throw new ArgumentNullException(nameof(sellerId));
    }
}
