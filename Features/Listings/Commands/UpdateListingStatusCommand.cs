using EcommerceApp.Models;
using EcommerceApp.Models.Enums;
using MediatR;

namespace EcommerceApp.Features.Listings.Commands;

public class UpdateListingStatusCommand : IRequest<Listing>
{
   
    public string ListingId { get; set; }

    public ListingStatus NewStatus { get; set; }
}