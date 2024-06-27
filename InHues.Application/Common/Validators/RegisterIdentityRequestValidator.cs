using FluentValidation;
using InHues.Domain.DTO.V1.Identity.Request;

namespace InHues.Application.Common.Validators
{
    public class RegisterIdentityRequestValidator : AbstractValidator<RegisterIdentityRequest>
    {
        public RegisterIdentityRequestValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Email is invalid")
                .NotEmpty().WithMessage("Email is required");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required");

        }
    }
}
