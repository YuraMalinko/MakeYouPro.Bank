namespace MakeYouPro.Bourse.CRM.Api.Models.Lead.Request
{
    public class UpdateLeadUsingLeadRequest
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? MiddleName { get; set; }

        public string? Surname { get; set; }

        public DateOnly? Birthday { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Comment { get; set; }
    }
}
