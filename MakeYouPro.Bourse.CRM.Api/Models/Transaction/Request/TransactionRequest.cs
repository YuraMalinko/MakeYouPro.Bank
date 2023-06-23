namespace MakeYouPro.Bourse.CRM.Api.Models.Transaction.Request
{
    public class TransactionRequest
    {
        public int LeadId { get; set; }

        public int AccountId { get; set; }

        public decimal Amount { get; set; }
    }
}
