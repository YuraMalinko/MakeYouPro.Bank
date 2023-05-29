using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Account.Request;
using MakeYouPro.Bourse.CRM.Core.Configurations.ISettings;
using MakeYouPro.Bourse.CRM.Core.Configurations.Settings;
using Microsoft.IdentityModel.Tokens;

namespace MakeYouPro.Bourse.CRM.Api.Validations
{
    public class CreateAccountValidator:AbstractValidator<AccountCreateRequest>
    {
        private readonly ICurrencySetting _currencySetting;

        public CreateAccountValidator(ICurrencySetting currencySetting)
        {
            _currencySetting = currencySetting;

            RuleFor(a => a.LeadId).NotEmpty().GreaterThan(0)
                .WithMessage("Lead ID not entered or less than zero");
            RuleFor(a => a.Currency).NotEmpty()
                .WithMessage("Currency type not entered")
                .Must(c =>_currencySetting.CurrencyVip.Contains(c) || _currencySetting.CurrencyStandart.Contains(c))
                .WithMessage("The entered currency code is incorrect or is not used by the program");
            RuleFor(a => a.Comment).Must(com => com.IsNullOrEmpty()).When(com => com.Comment is not null)
                .WithMessage("The comment is empty or consists of only spaces");
        }
    }
}
