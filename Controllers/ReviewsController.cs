using EcommerceApp.Features.Reviews.Commands;
using EcommerceApp.Features.Reviews.Queries;
using EcommerceApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return Ok(ApiResponse<string>.SuccessResponse(id, "Review submitted."));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }
    
    [HttpGet("seller/{sellerId}")]
    public async Task<IActionResult> GetBySeller(string sellerId)
    {
        try
        {
            var reviews = await _mediator.Send(new GetReviewsBySellerQuery(sellerId));
            return Ok(ApiResponse<List<Review>>.SuccessResponse(reviews));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<List<Review>>.Fail(ex.Message));
        }
    }

}