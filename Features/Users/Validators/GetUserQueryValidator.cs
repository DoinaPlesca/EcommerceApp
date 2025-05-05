using EcommerceApp.Features.Users.Queries;
using FluentValidation;

namespace EcommerceApp.Features.Users.Validators;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}