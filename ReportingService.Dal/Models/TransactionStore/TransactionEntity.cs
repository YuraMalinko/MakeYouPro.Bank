using System.ComponentModel.DataAnnotations;
using CoreRS.Enums;

namespace ReportingService.Dal.Models.TransactionStore
{
    public class TransactionEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public TransactionType Type { get; set; }
        [Required]
        public DateTime Time { get; set; }
    }
}
