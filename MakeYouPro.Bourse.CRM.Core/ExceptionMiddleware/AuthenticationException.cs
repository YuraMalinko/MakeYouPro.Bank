namespace MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string? message) : base($"A user authentication error has occurred for some reason : {message}") { }
    }
}
