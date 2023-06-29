namespace MakeYouPro.Bourse.CRM.Core.Configurations.ISettings
{
    public interface ICurrencySetting
    {
        string CurrencyDefault { get;}

        List<string> CurrencyStandart { get;}

        List<string> CurrencyVip { get;}

    }
}
