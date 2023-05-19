using MakeYouPro.Bank.CRM.Models.Lead.Response;

namespace MakeYouPro.Bank.CRM.Models.Account.Response
{
    public class AccountResponse
    {
        public int Id { get; set; }

        public LeadResponse Lead { get; set; }

        public int LeadId { get; set; }

        public DateTime DateCreate { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public int Status { get; set; }

        public string? Comment { get; set; }
    }
}
