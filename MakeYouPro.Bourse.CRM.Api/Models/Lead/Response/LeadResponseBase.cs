using MakeYouPro.Bank.CRM.Models.Account.Response;

namespace MakeYouPro.Bank.CRM.Models.Lead.Response
{
    public class LeadResponseBase
    {
        public int Id { get; set; }

        public int Role { get; set; }

        public int Status { get; set; }

        public string Name { get; set; }

        public string? MiddleName { get; set; }

        public string Surname { get; set; }

        public List<AccountResponse> Accounts { get; set; }
    }
}
