namespace MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware
{
    public class AccountDataException : Exception
    {
        private string _message = "";

        private object _objectException;

        public AccountDataException(object objectException, string? message) : base($"The following error occurred when transferring the account {objectException.GetType()}:{message}")
        {
            _objectException = objectException;
            if (message != null)
            {
                _message = message;
            }
        }
    }
}
