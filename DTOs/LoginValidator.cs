using FluentValidation;

namespace MediFlowApi.DTOs
{
    public class LoginValidator:AbstractValidator<LoginDto>
    {
       public LoginValidator()
        {
            RuleFor(x=>x.Email).NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is not correct");

            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}
