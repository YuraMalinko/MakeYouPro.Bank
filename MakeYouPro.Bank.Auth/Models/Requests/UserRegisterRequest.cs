namespace MakeYouPro.Bank.Api.Auth.Models.Requests
{
    public class UserRegisterRequest
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
