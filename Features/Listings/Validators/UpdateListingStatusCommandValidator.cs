using EcommerceApp.Features.Listings.Commands;
using FluentValidation;

namespace EcommerceApp.Features.Listings.Validators;

public class UpdateListingStatusCommandValidator : AbstractValidator<UpdateListingStatusCommand>
{
    public UpdateListingStatusCommandValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty().WithMessage("ListingId is required.");

        RuleFor(x => x.NewStatus)
            .IsInEnum().WithMessage("Invalid listing status.");
    }
}