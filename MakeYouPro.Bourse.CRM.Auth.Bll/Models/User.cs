using MakeYouPro.Bourse.CRM.Core.Enums;

namespace MakeYouPro.Bourse.CRM.Auth.Bll.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public LeadRoleEnum Role { get; set; }

        public LeadStatusEnum Status { get; set; }

        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public string UpdateUserPassword { get; set; }

        public override string ToString()
        {
            return $"{nameof(User)} Email: {Email} Role: {Role} Status {Status}";
        }
    }
}
