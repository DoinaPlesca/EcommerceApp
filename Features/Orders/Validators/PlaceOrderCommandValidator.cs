using EcommerceApp.Features.Orders.Commands;
using FluentValidation;

namespace EcommerceApp.Features.Orders.Validators;

public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
{
    public PlaceOrderCommandValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty().WithMessage("ListingId is required.");

        RuleFor(x => x.BuyerId)
            .NotEmpty().WithMessage("BuyerId is required.");
    }
}