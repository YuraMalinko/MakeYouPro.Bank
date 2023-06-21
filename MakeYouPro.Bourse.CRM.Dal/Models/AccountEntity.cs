using MakeYouPro.Bourse.CRM.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakeYouPro.Bourse.CRM.Dal.Models
{

    public class AccountEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(LeadId))]
        public LeadEntity Lead { get; set; }

        [Required]
        public int LeadId { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime DateCreate { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; }

        [Required]
        public AccountStatusEnum Status { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? Comment { get; set; }
    }
}