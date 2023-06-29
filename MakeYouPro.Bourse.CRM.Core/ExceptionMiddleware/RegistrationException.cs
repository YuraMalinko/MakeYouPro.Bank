namespace MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware
{
    public class RegistrationException : Exception
    {
        public RegistrationException(string? message) : base($"There was a registration error for some reasons : {message}") { }
    }
}