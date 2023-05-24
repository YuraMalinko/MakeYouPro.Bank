namespace MakeYouPro.Bourse.CRM.Api.Models.Lead.Request
{
    public class UpdateLeadUsingManagerRequest: UpdateLeadUsingLeadRequest
    {
        public string Email { get; set; }

        public string Citizenship { get; set; }

        public string Registration { get; set; }

        public string PassportNumber { get; set; }
    }
}