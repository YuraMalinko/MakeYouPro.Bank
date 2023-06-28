using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;

namespace MakeYouPro.Bourse.CRM.Api.Validations
{
    public class UpdateUsingManagerValidator : AbstractValidator<UpdateLeadUsingManagerRequest>
    {
        public UpdateUsingManagerValidator()
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
        }
    }
}
