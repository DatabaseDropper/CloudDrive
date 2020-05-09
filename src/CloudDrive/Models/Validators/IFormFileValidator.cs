using CloudDrive.Models.Input;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace CloudDrive.Models.Validators
{
    public class IFormFileValidator : AbstractValidator<IFormFile>
    {
        public IFormFileValidator()
        {
            RuleFor(x => x.FileName)
                            .NotNull()
                            .WithMessage("File name is in not correct.")
                            .NotEmpty()
                            .WithMessage("E-mail address is not correct.");

            RuleFor(x => x.Length)
                         .NotEqual(0)
                         .WithMessage("File is empty");
        }
    }
}
