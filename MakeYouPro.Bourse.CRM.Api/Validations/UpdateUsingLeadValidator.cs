using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;

namespace MakeYouPro.Bourse.CRM.Api.Validations
{
    public class UpdateUsingLeadValidator : AbstractValidator<UpdateLeadUsingLeadRequest>
    {
        public UpdateUsingLeadValidator()
        {
            RuleFor(lead => lead.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .MinimumLength(11).WithMessage("PhoneNumber must not be less than 11 characters.")
                .MaximumLength(16).WithMessage("PhoneNumber must not exceed 16 characters.");
        }
    }
}
