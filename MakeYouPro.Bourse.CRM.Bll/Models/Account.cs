using MakeYouPro.Bourse.CRM.Core.Enums;

namespace MakeYouPro.Bank.CRM.Bll.Models
{
    public class Account
    {
        public int Id { get; set; }

        public Lead Lead { get; set; }

        public int LeadId { get; set; }

        public DateTime DateCreate { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public AccountStatusEnum Status { get; set; }

        public string? Comment { get; set; }
    }
}
