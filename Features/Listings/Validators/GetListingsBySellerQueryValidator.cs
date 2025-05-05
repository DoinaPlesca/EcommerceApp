using EcommerceApp.Features.Listings.Queries;
using FluentValidation;

namespace EcommerceApp.Features.Listings.Validators;

public class GetListingsBySellerQueryValidator : AbstractValidator<GetListingsBySellerQuery>
{
    public GetListingsBySellerQueryValidator()
    {
        RuleFor(x => x.SellerId)
            .NotEmpty().WithMessage("SellerId is required.");
    }
}