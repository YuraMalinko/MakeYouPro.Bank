using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Account.Request;
using MakeYouPro.Bourse.CRM.Core.Configurations.ISettings;

namespace MakeYouPro.Bourse.CRM.Api.Validations
{
    public class AccountFilterValidation : AbstractValidator<AccountFilterRequest>
    {
        private readonly ICurrencySetting _currencySetting;

        public AccountFilterValidation(ICurrencySetting currencySetting)
        {
            _currencySetting = currencySetting;

            RuleFor(a => a.FromDateCreate).LessThanOrEqualTo(a => a.ToDateCreate).When(a => a.ToDateCreate != null).WithMessage("The beginning of the interval is greater than the end");
            RuleFor(a => a.FromBalace).LessThanOrEqualTo(a => a.ToBalace).When(a => a.ToDateCreate != null).WithMessage("The beginning of the interval is greater than the end")
                .GreaterThanOrEqualTo(0).When(a => a.FromBalace != null).WithMessage("Less than zero");
            RuleFor(a => a.ToBalace).GreaterThanOrEqualTo(0).When(a => a.FromBalace != null);
            RuleForEach(a => a.Currencies).Must(a => _currencySetting.CurrencyVip.Contains(a) || _currencySetting.CurrencyStandart.Contains(a))
                .When(a => a.Currencies.Count != 0)
                .WithMessage("The program cannot work with one of the entered currencies");
            RuleForEach(a => a.LeadsId).GreaterThanOrEqualTo(0)
                .WithMessage("One of the idi leads is less than zero");
        }
    }
}