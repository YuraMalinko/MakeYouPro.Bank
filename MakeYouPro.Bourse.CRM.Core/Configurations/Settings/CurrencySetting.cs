using MakeYouPro.Bourse.CRM.Core.Configurations.ISettings;

namespace MakeYouPro.Bourse.CRM.Core.Configurations.Settings
{
    public class CurrencySetting : ICurrencySetting
    {
        public  List<string> CurrencyStandart { get; set; } = new List<string>() { "RUB", "USD", "EUR" };

        public  List<string> CurrencyVip { get; set; } = new List<string>() { "RUB", "USD", "EUR" };
    }
}
