using CoreRS.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EntityFrameworkCore.EncryptColumn.Attribute;

namespace CoreRS.Models
{
    public class LeadDto
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public LeadRoleEnum Role { get; set; }

        [Required]
        public LeadStatusEnum Status { get; set; }


        [Required]
        [Column(TypeName = "datetime")]
        public DateTime DateCreate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string? MiddleName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        public string Surname { get; set; }

        [Required]
        public DateOnly Birthday { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string PhoneNumber { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(3)")]
        [MinLength(2)]
        public string Citizenship { get; set; }

        [Required]
        [EncryptColumn]
        [Column(TypeName = "nvarchar(30)")]
        public string PassportNumber { get; set; }

        [Required]
        [EncryptColumn]
        [Column(TypeName = "nvarchar(50)")]
        public string Registration { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? Comment { get; set; }

        public List<AccountDto> Accounts { get; set; } = new List<AccountDto>();
    }
}
