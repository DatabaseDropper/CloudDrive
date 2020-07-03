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
                .WithMessage("Adres e-mail ma nieprawidłowy format")
                .NotEmpty()
                .WithMessage("Adres e-mail ma nieprawidłowy format")
                .EmailAddress()
                .WithMessage("Adres e-mail ma nieprawidłowy format");

            RuleFor(x => x.UserName)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithMessage("Musisz podać nazwę użytkownika")
                .NotEmpty()
                .WithMessage("Musisz podać nazwę użytkownika");
                
            RuleFor(x => x.Login)
                .NotNull()
                .WithMessage("Musisz podać login")
                .NotEmpty()
                .WithMessage("Musisz podać login")
                .Length(6, 50)
                .WithMessage("Login musi podsiadać od 6 do 50 znaków");
            
            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage("Musisz podać hasło")
                .NotEmpty()
                .WithMessage("Musisz podać hasło")
                .Length(8, 256)
                .WithMessage("Hałso musi składać z 8 do 256 znaków");
        }
    }
}
