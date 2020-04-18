using CloudDrive.Models.Input;
using FluentValidation;
using System.Linq;

namespace CloudDrive.Models.Validators
{
    public class LoginInputValidator : AbstractValidator<LoginInput>
    {
        public LoginInputValidator()
        {
            RuleFor(x => x.LoginOrEmail)
                .NotNull()
                .WithMessage("Login or E-mail address is not correct.")
                .NotEmpty()
                .WithMessage("E-mail address is not correct.");

            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage("You need to provide your password.")
                .NotEmpty()
                .WithMessage("You need to provide your password.")
                .Length(8, 256)
                .WithMessage("Password's length must be greater or equal to 8 and not greater than 256.");
        }
    }
}
