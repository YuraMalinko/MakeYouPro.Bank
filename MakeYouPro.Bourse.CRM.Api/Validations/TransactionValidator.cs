using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Transaction.Request;

namespace MakeYouPro.Bourse.CRM.Api.Validations
{
    public class TransactionValidator : AbstractValidator<TransactionRequest>
    {
        public TransactionValidator()
        {
            RuleFor(t => t.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than 0");
        }
    }
}
