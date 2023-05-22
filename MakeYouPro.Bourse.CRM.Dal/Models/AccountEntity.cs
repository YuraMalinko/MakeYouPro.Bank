using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakeYouPro.Bource.CRM.Dal.Models
{
    
    public class AccountEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(LeadId))]
        public LeadEntity Lead { get; set; }

        public int LeadId { get; set; }

        [Required]
        public DateTime DateCreate { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }

        [Required]
        [Column(TypeName ="decimal(38,4)")]
        public decimal Balance { get; set; }

        [Required]
        public int Status { get; set; }

        [StringLength(2000, MinimumLength = 2)]
        public string? Comment { get; set; }
    }
}
