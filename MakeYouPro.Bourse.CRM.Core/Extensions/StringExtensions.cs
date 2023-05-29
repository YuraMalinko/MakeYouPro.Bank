namespace MakeYouPro.Bourse.CRM.Core.Extensions
{
    public static class StringExtensions
    {
        public static string FormatEmail(this string value)
        {
            return value.Trim().ToLower();
        }

        public static string FormatPassportNumber(this string value)
        {
            return value.Replace(" ", string.Empty).ToUpper();
        }

        public static string FormatPhoneNumber(this string value)
        {
            return value.Replace(" ", string.Empty)
                        .Replace("-", string.Empty)
                        .Replace("+", string.Empty);
        }

        public static string FormatName(this string value)
        {
            if (value.Length > 1)
            {
                return value.Trim()
                            .Substring(0, 1).ToUpper() + value.Trim().Substring(1).ToLower();
            }
            else
            {
                return value.ToUpper();
            }
        }

        public static string FormatCitizenship(this string value)
        {
            return value.Replace(" ", string.Empty).ToUpper();
        }
    }
}
