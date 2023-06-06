using MakeYouPro.Bourse.CRM.Core.Enums;

namespace MakeYouPro.Bourse.CRM.Api.Models.Lead.Response
{
    public class LeadResponseMinInfo
    {
        public int Id { get; set; }

        public LeadRoleEnum Role { get; set; }

        public LeadStatusEnum Status { get; set; }

        public string Name { get; set; }

        public string? MiddleName { get; set; }

        public string Surname { get; set; }
    }
}
