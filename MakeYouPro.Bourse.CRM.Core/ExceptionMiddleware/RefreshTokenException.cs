namespace MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware
{
    public class RefreshTokenException : Exception
    {
        public RefreshTokenException(string? message) : base($"An error occurred while working with the refresh token:{message!}")
        {

        }
    }
}
