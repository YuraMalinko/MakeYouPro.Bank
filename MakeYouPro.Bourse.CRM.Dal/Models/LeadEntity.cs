using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakeYouPro.Bource.CRM.Dal.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class LeadEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Role { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime DateCreate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        [MinLength(5)]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [MinLength(5)]
        public string? MiddleName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        [MinLength(5)]
        public string Surname { get; set; }

        [Required]
        [Phone]
        [Column(TypeName = "nvarchar(30)")]
        [MinLength(5)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [Column(TypeName = "nvarchar(100)")]
        [MinLength(5)]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(3)")]
        [MinLength(3)]
        public string Citizenship { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        [MinLength(5)]
        public string PassportSeries { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        [MinLength(5)]
        public string PassportNumber { get; set; }

        [StringLength(500, MinimumLength = 3)]
        public string? Comment { get; set; }

        public virtual List<AccountEntity> Accounts { get; set; } = new List<AccountEntity>();
    }
}
