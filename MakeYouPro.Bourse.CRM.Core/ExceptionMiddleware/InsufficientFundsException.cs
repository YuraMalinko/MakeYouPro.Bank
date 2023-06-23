namespace MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException() : base("There are not enough funds on the account to carry out this operation")
        {
        }
    }
}
