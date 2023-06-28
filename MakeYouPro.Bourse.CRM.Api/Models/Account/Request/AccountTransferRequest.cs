namespace MakeYouPro.Bourse.CRM.Api.Models.Account.Request
{
    public class AccountTransferRequest
    {
        public int AccountId { get; set; }

        public int LeadId { get; set; }

        public string Currency { get; set; }
    }
}
