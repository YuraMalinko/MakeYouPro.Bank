namespace MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware
{
    public class AccountArgumentException : Exception
    {
        private string _message;

        public AccountArgumentException(string? message) : base($"The following error occurred when transferring the account:{message}")
        {
            if (message != null)
            {
                _message = message;
            }
        }
    }
}
