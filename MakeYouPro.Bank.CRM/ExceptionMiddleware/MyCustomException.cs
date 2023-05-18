namespace MakeYouPro.Bank.CRM.ExceptionMiddleware
{
    public class MyCustomException : Exception
    {
        public MyCustomException(string message) : base(message)
        {
        }
    }
}
