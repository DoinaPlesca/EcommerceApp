using EcommerceApp.Features.Listings.Commands;
using EcommerceApp.Features.Listings.Queries;
using EcommerceApp.Models;
using EcommerceApp.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ListingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateListingCommand command)
    {
        try
        {
            var listing = await _mediator.Send(command);
            return Ok(ApiResponse<Listing>.SuccessResponse(listing, "Listing created."));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetListingsQuery query)
    {
        try
        {
            var result = await _mediator.Send(query);
            return Ok(ApiResponse<PagedResult<Listing>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PagedResult<Listing>>.Fail(ex.Message));
        }
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateListingStatusCommand command)
    {
        if (id != command.ListingId)
            return BadRequest("ID in route does not match body.");

        try
        {
            var updatedListing = await _mediator.Send(command);
            if (updatedListing == null)
                return NotFound(ApiResponse<string>.Fail("Listing not found or status not updated."));
           
            return Ok(ApiResponse<Listing>.SuccessResponse(updatedListing, "Status updated."));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }
    
    [HttpGet("seller/{sellerId}")]
    public async Task<IActionResult> GetBySeller(string sellerId)
    {
        if (string.IsNullOrWhiteSpace(sellerId))
            return BadRequest(ApiResponse<List<Listing>>.Fail("SellerId is required."));

        try
        {
            var listings = await _mediator.Send(new GetListingsBySellerQuery(sellerId));

            return Ok(ApiResponse<List<Listing>>.SuccessResponse(listings));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<List<Listing>>.Fail("An error occurred: " + ex.Message));
        }
    }


}