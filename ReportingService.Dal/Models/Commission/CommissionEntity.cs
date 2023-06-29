using CoreRS.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReportingService.Dal.Models.TransactionStore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportingService.DAL.ModelsDAL.Commissions
{
    public class CommissionEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public TransactionType Type { get; set; }

        [Required]
        [Column(TypeName = "decimal(38,4)")]
        public decimal Amount { get; set; }

        [Required]
        [Range(2, 2)]
        public int OperationDay { get; set; }

        [Required]
        [Range(2, 2)]
        public int OperationMonth { get; set; }

        [Required]
        [Range(4, 4)]
        public int OperationYear { get; set; }

        [Required]
        [Column(TypeName = "time")]
        public TimeOnly OperationTime { get; set; }

        [Required]
        [ForeignKey(nameof(TransactionId))]
        public TransactionEntity Transaction { get; set; }

        public int TransactionId { get; set; }
    }
}
