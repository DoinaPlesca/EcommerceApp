using EcommerceApp.Features.Listings.Querie;
using EcommerceApp.Shared.Constants;
using FluentValidation;

namespace EcommerceApp.Features.Listings.Validators;

public class GetListingsQueryValidator : AbstractValidator<GetListingsQuery>
{
    public GetListingsQueryValidator()
    {
        RuleFor(x => x.Search)
            .MinimumLength(2)
            .When(x => !string.IsNullOrWhiteSpace(x.Search))
            .WithMessage("Search term must be at least 2 characters.");

        RuleFor(x => x.Category)
            .Must(BeAValidCategory)
            .When(x => !string.IsNullOrWhiteSpace(x.Category))
            .WithMessage("Invalid category.");
        
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");

        RuleFor(x => x.SortDirection)
            .Must(dir => string.IsNullOrEmpty(dir) || dir.ToLower() == "asc" || dir.ToLower() == "desc")
            .WithMessage("SortDirection must be 'asc' or 'desc'.");

        RuleFor(x => x.SortBy)
            .Must(field => string.IsNullOrEmpty(field) || ListingsConstants.AllowedSortFields.Contains(field.ToLower()))
            .WithMessage($"SortBy must be one of: {string.Join(", ", ListingsConstants.AllowedSortFields)}.");
    }

    private bool BeAValidCategory(string category)
    {
        return ListingsConstants.AllowedCategories.Contains(category);
    }
}