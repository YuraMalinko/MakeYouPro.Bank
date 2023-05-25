using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Response;

namespace MakeYouPro.Bourse.CRM.Api.Validations
{
    public class RegistrateValidator : AbstractValidator<CreateLeadRequest>
    {
        public RegistrateValidator() 
        {
            RuleFor(lead => lead.Email)
                .NotEmpty().WithMessage("Email address is required")
                .Must(em => !em.Trim().Any(char.IsWhiteSpace))
                .EmailAddress().WithMessage("A valid email is required");
        }
    }
}
