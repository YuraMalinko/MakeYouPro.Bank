using CoreRS.Enums;

namespace ReportingService.Dal.Models.TransactionStore
{
    public class TransactionEntity
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public DateTime Time { get; set; }
    }
}
