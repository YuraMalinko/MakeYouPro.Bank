using MakeYouPro.Bource.CRM.Core.Enums;

namespace MakeYouPro.Bank.CRM.Bll.Models
{
    public class Lead
    {
        public int Id { get; set; }

        public LeadRoleEnum Role { get; set; }

        public LeadStatusEnum Status { get; set; }

        public DateTime DateCreate { get; set; }

        public string Name { get; set; }

        public string? MiddleName { get; set; }

        public string Surname { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Citizenship { get; set; }

        public string PassportNumber { get; set; }

        public string Registration { get; set; }

        public string? Comment { get; set; }

        public string? Password { get; set; }

        public bool IsDeleted { get; set; }

        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}
