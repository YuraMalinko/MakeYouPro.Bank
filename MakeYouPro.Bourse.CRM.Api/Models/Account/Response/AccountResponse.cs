using MakeYouPro.Bank.CRM.Models.Lead.Response;
using MakeYouPro.Bourse.CRM.Core.Enums;

namespace MakeYouPro.Bank.CRM.Models.Account.Response
{
    public class AccountResponse
    {
        public int Id { get; set; }

        public int LeadId { get; set; }

        //public LeadResponseBase Lead { get; set; }

        public DateTime DateCreate { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public AccountStatusEnum Status { get; set; }

        public string? Comment { get; set; }
    }
}
