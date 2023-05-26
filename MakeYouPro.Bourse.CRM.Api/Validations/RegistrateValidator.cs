using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Response;
using System.Text.RegularExpressions;

namespace MakeYouPro.Bourse.CRM.Api.Validations
{
    public class RegistrateValidator : AbstractValidator<CreateLeadRequest>
    {
        public RegistrateValidator() 
        {
            RuleFor(lead => lead.Email.Trim())
                .NotEmpty().WithMessage("Email address is required")
                .Must(em => !em.Any(char.IsWhiteSpace))
                .Matches(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                         + "@"
                         + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$")
                .Matches(@"^[^А-Яа-я]+$").WithMessage("Your password must contain english letter or digitals")
                .EmailAddress().WithMessage("A valid email is required");
            RuleFor(lead => lead.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .MinimumLength(11).WithMessage("PhoneNumber must not be less than 11 characters.")
                .MaximumLength(16).WithMessage("PhoneNumber must not exceed 16 characters.");
            RuleFor(lead => lead.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must not be less than 6 characters")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.");
        }
    }
}
