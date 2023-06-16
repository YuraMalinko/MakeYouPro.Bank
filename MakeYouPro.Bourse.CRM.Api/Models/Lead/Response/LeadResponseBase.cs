using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Models.Account.Response;

namespace MakeYouPro.Bourse.CRM.Models.Lead.Response
{
    public class LeadResponseBase
    {
        public int Id { get; set; }

        public LeadRoleEnum Role { get; set; }

        public LeadStatusEnum Status { get; set; }

        public string Name { get; set; }

        public string? MiddleName { get; set; }

        public string Surname { get; set; }

        public DateOnly Birthday { get; set; }

        public List<AccountResponse> Accounts { get; set; }
    }
}
