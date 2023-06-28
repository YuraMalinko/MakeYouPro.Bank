namespace MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware
{
    public class TransactionException : Exception
    {
        public TransactionException() : base("Transaction failed. Try again later")
        {
        }
    }
}
