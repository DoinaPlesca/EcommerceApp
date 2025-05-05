using EcommerceApp.Models;
using MediatR;

namespace EcommerceApp.Features.Listings.Querie;

public class GetListingsQuery : IRequest<List<Listing>>
{
    public string? Search { get; set; }
    public string? Category { get; set; }
}