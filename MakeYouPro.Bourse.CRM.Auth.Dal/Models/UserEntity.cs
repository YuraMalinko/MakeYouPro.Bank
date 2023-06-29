using MakeYouPro.Bourse.CRM.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakeYouPro.Bourse.CRM.Auth.Dal.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string Password { get; set; }

        [Required]
        public LeadRoleEnum Role { get; set; }

        [Required]
        public LeadStatusEnum Status { get; set; }

        public List<RefreshTokenEntity> RefreshTokens { get; set; } = new List<RefreshTokenEntity>();

        public override string ToString()
        {
            return $"{nameof(UserEntity)} Email: {Email} Role: {Role} Status {Status}";
        }

        public override bool Equals(object? obj)
        {
            return obj is UserEntity user &&
                Id==user.Id &&
                Email==user.Email &&
                Password==user.Password &&
                Role==user.Role &&
                Status==user.Status &&
                RefreshTokens.SequenceEqual(user.RefreshTokens);
        }
    }
}
