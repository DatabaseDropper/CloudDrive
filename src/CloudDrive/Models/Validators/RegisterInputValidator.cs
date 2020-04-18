using CloudDrive.Models.Input;
using FluentValidation;
using System.Linq;

namespace CloudDrive.Models.Validators
{
    public class RegisterInputValidator : AbstractValidator<RegisterInput>
    {
        public RegisterInputValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage("E-mail address is not correct.")
                .NotEmpty()
                .WithMessage("E-mail address is not correct.")
                .EmailAddress()
                .WithMessage("E-mail address is not correct.");

            RuleFor(x => x.UserName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithMessage("You need to provide your user name.")
                .NotEmpty()
                .WithMessage("You need to provide your user name.");
                
            RuleFor(x => x.Login)
                .NotNull()
                .WithMessage("You need to provide your login.")
                .NotEmpty()
                .WithMessage("You need to provide your login.")
                .Length(6, 50)
                .WithMessage("Login's length must be greater or equal to 6 and not greater than 50.");
            
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
