namespace ReportingService.Bll.Models.CRM
{
    public class Account
    {
        public int Id { get; set; }

        public Lead Lead { get; set; }

        public int LeadId { get; set; }

        public DateTime DateCreate { get; set; }

        public string Currency { get; set; }

        public decimal Balance { get; set; }

        public int Status { get; set; }

        public string? Comment { get; set; }
    }
}
