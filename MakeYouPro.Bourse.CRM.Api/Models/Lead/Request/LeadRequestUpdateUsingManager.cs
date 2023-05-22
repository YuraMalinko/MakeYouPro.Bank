namespace MakeYouPro.Bourse.CRM.Api.Models.Lead.Request
{
    public class LeadRequestUpdateUsingManager: LeadRequestUpdateUsingLead
    {
        public string Email { get; set; }

        public string Citizenship { get; set; }

        public string PassportNumber { get; set; }
    }
}