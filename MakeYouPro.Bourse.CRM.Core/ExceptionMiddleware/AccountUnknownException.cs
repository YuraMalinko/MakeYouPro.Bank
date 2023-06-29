namespace MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware
{
    public class AccountUnknownException : Exception
    {
        public AccountUnknownException(string? message) : base($"An unknown error occurred while working with the Account," +
            $" Administrator intervention is required : {message}")
        {
        }
    }
}
