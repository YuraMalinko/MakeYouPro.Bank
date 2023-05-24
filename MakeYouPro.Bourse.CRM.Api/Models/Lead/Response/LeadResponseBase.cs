using MakeYouPro.Bank.CRM.Models.Account.Response;
using MakeYouPro.Bource.CRM.Core.Enums;

namespace MakeYouPro.Bank.CRM.Models.Lead.Response
{
    public class LeadResponseBase
    {
        public int Id { get; set; }

        public LeadRoleEnum Role { get; set; }

        public LeadStatusEnum Status { get; set; }

        public string Name { get; set; }

        public string? MiddleName { get; set; }

        public string Surname { get; set; }

        public List<AccountResponse> Accounts { get; set; }
    }
}
