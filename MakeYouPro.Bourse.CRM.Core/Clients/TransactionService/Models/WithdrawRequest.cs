namespace MakeYouPro.Bourse.CRM.Core.Clients.TransactionService.Models
{
    public class WithdrawRequest
    {
        public int AccountId { get; set; }

        public decimal Amount { get; set; }
    }
}
