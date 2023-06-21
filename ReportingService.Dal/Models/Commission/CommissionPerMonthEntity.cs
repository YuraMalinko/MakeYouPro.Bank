using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReportingService.DAL.ModelsDAL.Commissions
{
    public class CommissionPerMonthEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        [Range(2, 2)]
        public int OperationMonth { get; set; }

        [Required]
        [Range(4, 4)]
        public int OperationYear { get; set; }

        [Required]
        [Column(TypeName = "decimal(38,4)")]
        public decimal AmountCommission { get; set; }
    }
}
