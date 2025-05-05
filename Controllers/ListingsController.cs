using EcommerceApp.Features.Listings.Commands;
using EcommerceApp.Features.Listings.Querie;
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
            var id = await _mediator.Send(command);
            return Ok(ApiResponse<string>.SuccessResponse(id, "Listing created."));
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
            var success = await _mediator.Send(command);
            if (!success)
                return NotFound(ApiResponse<bool>.Fail("Listing not found or status not updated."));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Status updated."));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<bool>.Fail(ex.Message));
        }
    }
}