namespace MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware
{
    public class AuthorizationException : Exception
    {
        public AuthorizationException(string? message, string? user) : base($"User authentication error occurred {user} for some reason : {message}")
        {

        }
    }
}
