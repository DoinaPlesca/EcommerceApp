using EcommerceApp.Models;
using MediatR;

namespace EcommerceApp.Features.Listings.Queries;

public class GetListingsBySellerQuery : IRequest<List<Listing>>
{
    public string SellerId { get; set; }

    public GetListingsBySellerQuery(string sellerId)
    {
        SellerId = sellerId;
    }
}