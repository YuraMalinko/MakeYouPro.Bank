using MakeYouPro.Bourse.CRM.Api.Models.Account.Request;

namespace MakeYouPro.Bourse.CRM.Api.Models.Transaction.Request
{
    public class TransferTransactionRequest
    {
        public AccountTransferRequest AccountSource { get; set; }

        public AccountTransferRequest AccountDestination { get; set; }

        public decimal Amount { get; set; }
    }
}
