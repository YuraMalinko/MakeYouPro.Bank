namespace MakeYouPro.Bourse.CRM.Core.Clients.TransactionService.Models
{
    public class TransferRequest
    {
        public int AccountId { get; set; }

        public decimal Amount { get; set; }

        public string MoneyType { get; set; }

        public int TargetAccountId { get; set; }

        public string TargetMoneyType { get; set; }
    }
}
