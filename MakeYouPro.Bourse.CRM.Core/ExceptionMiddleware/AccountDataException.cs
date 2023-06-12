namespace MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware
{
    public class AccountDataException : Exception
    {
        private string _message;

        public AccountDataException(string? message) : base($"The following error occurred when transferring the account:{message}")
        {
            if (message != null)
            {
                _message = message;
            }
        }
    }
}
