using System.Text.Json;
using EcommerceApp.Models;

namespace EcommerceApp.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.ContentType = "application/json";

            var response = context.Response;
            response.StatusCode = ex switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                KeyNotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            var apiResponse = ApiResponse<string>.Fail(ex.Message);
            var result = JsonSerializer.Serialize(apiResponse);
            await context.Response.WriteAsync(result);
        }
    }
}
