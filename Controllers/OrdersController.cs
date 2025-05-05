using EcommerceApp.Features.Orders.Commands;
using EcommerceApp.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderCommand command)
    {
        try
        {
            var id = await _mediator.Send(command);
            return Ok(ApiResponse<string>.SuccessResponse(id, "Order placed."));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.Fail(ex.Message));
        }
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(string id, [FromBody] UpdateOrderStatusCommand command)
    {
        if (id != command.OrderId)
            return BadRequest(ApiResponse<bool>.Fail("ID in URL and body must match."));

        try
        {
            var success = await _mediator.Send(command);
            if (!success)
                return NotFound(ApiResponse<bool>.Fail("Order not found or not updated."));

            return Ok(ApiResponse<bool>.SuccessResponse(true, "Order status updated."));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<bool>.Fail(ex.Message));
        }
    }

}