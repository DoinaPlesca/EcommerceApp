using EcommerceApp.Models.Enums;
using MediatR;

namespace EcommerceApp.Features.Listings.Commands;

public class UpdateListingStatusCommand : IRequest<bool>
{
   
    public string ListingId { get; set; }

    public ListingStatus NewStatus { get; set; }
}