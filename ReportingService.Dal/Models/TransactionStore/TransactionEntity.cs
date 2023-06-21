using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ReportingService.Dal.Models.CRM;

namespace ReportingService.Dal.Models.TransactionStore
{
    public class TransactionEntity
    {
        [Key]
        public int Id { get; set; }

        public string Type { get; set; }

        [Required]
        [Column(TypeName = "decimal(38,4)")]
        public decimal Amount { get; set; }

        public DateTime Time { get; set; }

        [Required]
        [ForeignKey(nameof(AccountId))]
        public AccountEntity Account { get; set; }

        public int AccountId { get; set; }
    }
}
