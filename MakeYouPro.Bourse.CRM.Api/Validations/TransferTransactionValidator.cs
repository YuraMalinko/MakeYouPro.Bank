using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Transaction.Request;

namespace MakeYouPro.Bourse.CRM.Api.Validations
{
    public class TransferTransactionValidator : AbstractValidator<TransferTransactionRequest>
    {
        public TransferTransactionValidator()
        {
            RuleFor(t => t.Amount)
                .GreaterThan(0)
                .WithMessage("CommissionAmount must be greater than 0");
            RuleFor(t => t.AccountSource.LeadId)
                    .Equal(t => t.AccountDestination.LeadId)
                    .WithMessage("TransferTransaction can only be carried out between Accounts of the same Lead");
            RuleFor(t => t.AccountSource.Currency)
                    .NotEqual(t => t.AccountDestination.Currency)
                    .WithMessage("To carry out a transfer transaction, the currencies of the Accounts must be different");
        }
    }
}
