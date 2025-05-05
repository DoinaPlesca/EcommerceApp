using EcommerceApp.Models;
using EcommerceApp.Models.DTOs;
using EcommerceApp.Models.Enums;
using MediatR;

namespace EcommerceApp.Features.Listings.Querie;

public class GetListingsQuery : IRequest<PagedResult<Listing>>
{
    public string? Search { get; set; }
    public string? Category { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }

    public ListingStatus? Status { get; set; } 
}