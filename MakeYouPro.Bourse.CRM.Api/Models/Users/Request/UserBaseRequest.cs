namespace MakeYouPro.Bourse.CRM.Api.Models.Users.Request
{
    public class UserBaseRequest
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
