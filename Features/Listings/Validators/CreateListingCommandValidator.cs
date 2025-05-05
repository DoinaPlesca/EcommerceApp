using EcommerceApp.Features.Listings.Commands;
using FluentValidation;

namespace EcommerceApp.Features.Listings.Validators;

public class CreateListingCommandValidator : AbstractValidator<CreateListingCommand>
{
    public CreateListingCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .Length(3, 100).WithMessage("Title must be between 3 and 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Length(10, 1000).WithMessage("Description must be between 10 and 1000 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.SellerId)
            .NotEmpty().WithMessage("SellerId is required.");

        RuleFor(x => x.ImageUrls)
            .NotNull().WithMessage("At least one image is required.")
            .Must(imgs => imgs.Count > 0).WithMessage("At least one image URL is required.");
    }
}