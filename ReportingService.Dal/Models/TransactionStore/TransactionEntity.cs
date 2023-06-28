using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ReportingService.Dal.Models.CRM;
using CoreRS.Enums;

namespace ReportingService.Dal.Models.TransactionStore
{
    public class TransactionEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int IdOutside { get; set; }

        public TransactionType Type { get; set; }

        [Required]
        [Column(TypeName = "decimal(38,4)")]
        public decimal Amount { get; set; }

        public DateTime DataTime { get; set; }

        [Required]
        [ForeignKey(nameof(AccountId))]
        public AccountEntity Accounts { get; set; }

        public int AccountId { get; set; }
    }
}
