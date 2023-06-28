using EntityFrameworkCore.EncryptColumn.Attribute;
using MakeYouPro.Bourse.CRM.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MakeYouPro.Bourse.CRM.Dal.Models
{
    //  [Index(nameof(Email), IsUnique = true)]
    [Index("PhoneNumber", "Email", "PassportNumber", "Citizenship", IsUnique = true, Name = "EmailPhonePassportIndex")]
    public class LeadEntity
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
        [Column(TypeName = "nvarchar(50)")]
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

        public List<AccountEntity> Accounts { get; set; } = new List<AccountEntity>();

        public override string ToString()
        {
            return $"ID:{Id} Name: {Name} {MiddleName} {Surname}";
        }
    }
}