using EcommerceApp.Models;
using EcommerceApp.Services;
using MediatR;

namespace EcommerceApp.Features.Listings.Commands;

public class CreateListingCommand : IRequest<Listing>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string SellerId { get; set; }
    public string Category { get; set; }
    public string Condition { get; set; }
    public List<string> ImageUrls { get; set; }
}
