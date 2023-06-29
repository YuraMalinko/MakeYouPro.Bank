namespace MakeYouPro.Bourse.CRM.Auth.Bll.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }

        public DateTime? Expires { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? Revoked { get; set; }

        public bool IsActive { get; set; }

        public bool IsExpired { get; set; }

    }
}
