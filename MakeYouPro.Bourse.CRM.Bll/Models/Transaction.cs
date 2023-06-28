namespace MakeYouPro.Bourse.CRM.Bll.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public int LeadId { get; set; }

        public decimal Amount { get; set; }

        public string Type { get; set; }

        public DateTime Time { get; set; }
    }
}
