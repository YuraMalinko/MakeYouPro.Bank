﻿using MakeYouPro.Bourse.CRM.Core.Configurations.ISettings;
using MakeYouPro.Bourse.CRM.Core.Configurations.Settings;

namespace MakeYouPro.Bourse.CRM.Api
{
    public class InjectionConfiguration
    {
        public InjectionConfiguration(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICurrencySetting, CurrencySetting>();
            builder.Services.AddScoped<IAccountSetting, AccountSetting>();
        }
    }
}
