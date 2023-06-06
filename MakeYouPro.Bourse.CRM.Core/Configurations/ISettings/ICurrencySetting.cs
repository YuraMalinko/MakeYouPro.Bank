namespace MakeYouPro.Bourse.CRM.Core.Configurations.ISettings
{
    public interface ICurrencySetting
    {
        List<string> CurrencyStandart { get; set; }

        List<string> CurrencyVip { get; set; }
    }
}
