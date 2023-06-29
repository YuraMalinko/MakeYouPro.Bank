namespace MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware
{
    public class WritingDataToServerException : Exception
    {
        public WritingDataToServerException(string? message) : base($"For an unknown reason, data was not written to the server:{message}")
        {
        }
    }
}
