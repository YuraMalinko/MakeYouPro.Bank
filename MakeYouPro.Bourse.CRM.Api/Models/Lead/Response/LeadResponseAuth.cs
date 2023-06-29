using MakeYouPro.Bourse.CRM.Models.Lead.Response;

namespace MakeYouPro.Bourse.CRM.Api.Models.Lead.Response
{
    public class LeadResponseAuth:LeadResponseBase
    {
        public string Token { get; set; }
    }
}
