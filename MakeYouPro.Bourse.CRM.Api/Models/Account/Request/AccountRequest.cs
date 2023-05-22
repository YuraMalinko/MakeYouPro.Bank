namespace MakeYouPro.Bank.CRM.Models.Account.Request
{
    public class AccountRequest
    {
        public int LeadId { get; set; }

        public DateTime DateCreate { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public int Status { get; set; }

        public string? Comment { get; set; }
    }
}
