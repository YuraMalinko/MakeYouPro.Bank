namespace MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware
{
    public class UnsuitableCurrencyException : Exception
    {
        public string CurrencyName { get; set; }

        public UnsuitableCurrencyException(string currencyName) : base($"{currencyName} not suitable for this operation")
        {
            CurrencyName = currencyName;
        }
    }
}
