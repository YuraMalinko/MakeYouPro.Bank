using MakeYouPro.Bourse.CRM.Core.Configurations.ISettings;
using MakeYouPro.Bourse.CRM.Core.Configurations.Settings;

namespace MakeYouPro.Bourse.CRM.Api
{
    public class InjectionConfiguration
    {
        public InjectionConfiguration(WebApplicationBuilder builder)
        {
            //var currencySettings = builder.Configuration.GetSection("CurrencySetting").Get<CurrencySetting>();

            builder.Services.AddScoped<ICurrencySetting,CurrencySetting>();
        }
    }
}
