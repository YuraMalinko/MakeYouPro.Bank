using MakeYouPro.Bank.CRM.Models.Lead.Response;

namespace MakeYouPro.Bourse.CRM.Api.Models.Lead.Response
{
    public class LeadResponseInfo:LeadResponseBase
    {
        public DateTime? DateCreate { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Citizenship { get; set; }

        public string? PassportSeries { get; set; }

        public string? PassportNumber { get; set; }

        public string? Comment { get; set; }
    }
}
