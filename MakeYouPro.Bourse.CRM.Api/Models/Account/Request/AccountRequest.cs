using MakeYouPro.Bourse.CRM.Core.Enums;

namespace MakeYouPro.Bourse.CRM.Models.Account.Request
{
    public class AccountRequest
    {
        public int LeadId { get; set; }

        public DateTime DateCreate { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public AccountStatusEnum Status { get; set; }

        public string? Comment { get; set; }
    }
}
