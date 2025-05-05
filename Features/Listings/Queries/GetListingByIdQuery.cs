using EcommerceApp.Models;
using MediatR;

namespace EcommerceApp.Features.Listings.Queries;

public class GetListingByIdQuery :IRequest<Listing>
{
    public string ListingId { get; set; }
}