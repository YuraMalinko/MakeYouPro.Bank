namespace MakeYouPro.Bourse.CRM.Api.Models.Account.Request
{
    public class AccountCreateRequest
    {
        public int LeadId { get; set; }

        public string Currency { get; set; }

        public string? Comment { get; set; }
    }
}
