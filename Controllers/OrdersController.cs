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
            var order = await _mediator.Send(command);
            return Ok(ApiResponse<Order>.SuccessResponse(order, "Order placed."));
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
            return BadRequest(ApiResponse<string>.Fail("ID in URL and body must match."));

        try
        {
            var updatedOrder = await _mediator.Send(command);
            
            if (updatedOrder == null)
                return NotFound(ApiResponse<string>.Fail("Order not found or not updated."));

            return Ok(ApiResponse<Order>.SuccessResponse(updatedOrder, "Order status updated."));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<bool>.Fail(ex.Message));
        }
    }

}