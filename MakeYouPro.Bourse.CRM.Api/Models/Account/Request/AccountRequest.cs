using MakeYouPro.Bank.CRM.Models.Lead.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;

namespace MakeYouPro.Bank.CRM.Models.Account.Request
{
    public class AccountRequest
    {
        public LeadRequestUpdateUsingLead Lead { get; set; }

        public int LeadId { get; set; }

        public DateTime DateCreate { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public int Status { get; set; }

        public string? Comment { get; set; }
    }
}
