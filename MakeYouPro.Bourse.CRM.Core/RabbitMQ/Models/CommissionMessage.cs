namespace MakeYouPro.Bourse.CRM.Core.RabbitMQ.Models
{
    public class CommissionMessage
    {
        public int TransactionId { get; set; }

        public decimal CommissionAmount { get; set; }
    }
}
