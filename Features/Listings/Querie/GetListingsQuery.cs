using EcommerceApp.Models;
using MediatR;

namespace EcommerceApp.Features.Listings.Querie;

public class GetListingsQuery : IRequest<List<Listing>>
{
    public string? Search { get; set; }
    public string? Category { get; set; }

    public int Page { get; set; } = 1;          // page number
    public int PageSize { get; set; } = 10;     // Items per page

    public string? SortBy { get; set; }         // "Price", "CreatedAt"
    public string? SortDirection { get; set; }  // "asc" or "desc"
}