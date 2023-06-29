using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MakeYouPro.Bourse.CRM.Auth.Dal.Models
{
    public class RefreshTokenEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(1000)")]
        public string Token { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime Expires { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime Created { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? Revoked { get; set; }

        [Required]
        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; }

        [Required]
        public int UserId { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;

        public bool IsActive => Revoked == null && !IsExpired;


        public override bool Equals(object? obj)
        {
            return obj is RefreshTokenEntity token &&
                Id == token.Id &&
                Token == token.Token &&
                Expires == token.Expires &&
                IsExpired == token.IsExpired &&
                Created == token.Created &&
                Revoked == token.Revoked &&
                IsActive == token.IsActive &&
                UserId == token.UserId;

        }
    }
}
