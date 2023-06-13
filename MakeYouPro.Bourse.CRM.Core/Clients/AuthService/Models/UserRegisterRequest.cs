namespace MakeYouPro.Bourse.CRM.Core.Clients.AuthService.Models
{
    public class UserRegisterRequest
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
