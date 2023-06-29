using MakeYouPro.Bourse.CRM.Core.Configurations.ISettings;

namespace MakeYouPro.Bourse.CRM.Core.Configurations.Settings
{
    public class CurrencySetting : ICurrencySetting
    {
        public string CurrencyDefault { get; set; } = "RUB";

        public List<string> CurrencyStandart { get;} = new List<string>() { "RUB","USD", "EUR" };

        public List<string> CurrencyVip { get;} = new List<string>() { "RUB","USD", "EUR", "JPY", "CNY", "RSD", "BGN", "ARS" };
    }
}
