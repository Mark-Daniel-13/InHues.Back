using FluentValidation;
using InHues.Domain.DTO.V1.Identity.Request;

namespace InHues.Application.Common.Validators
{
    public class LoginIdentityRequestValidator : AbstractValidator<LoginIdentityRequest>
    {
        public LoginIdentityRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}
