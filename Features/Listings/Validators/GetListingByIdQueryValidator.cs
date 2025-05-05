
using EcommerceApp.Features.Listings.Queries;
using FluentValidation;

namespace EcommerceApp.Features.Listings.Validators;

public class GetListingByIdQueryValidator : AbstractValidator<GetListingByIdQuery>
{
    public GetListingByIdQueryValidator()
    {
        RuleFor(x => x.ListingId)
            .NotEmpty().WithMessage("ListingId is required.");
    }
}