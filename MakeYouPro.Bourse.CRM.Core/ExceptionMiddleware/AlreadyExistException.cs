
namespace MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware
{
    public class AlreadyExistException : Exception
    {
        public string PropertyName { get; set; }

        public AlreadyExistException(string propertyName) : base($"{propertyName} is already exist in dataBase")
        {
            PropertyName = propertyName;
        }
    }
}
