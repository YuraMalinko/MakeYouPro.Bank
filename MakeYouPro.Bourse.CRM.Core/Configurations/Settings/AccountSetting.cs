﻿using MakeYouPro.Bourse.CRM.Core.Configurations.ISettings;
using MakeYouPro.Bourse.CRM.Core.Enums;

namespace MakeYouPro.Bourse.CRM.Core.Configurations.Settings
{
    public class AccountSetting : IAccountSetting
    {
        public decimal StartBalance { get; set; } = 0;

        public AccountStatusEnum StartAccountStatus { get; set; } = AccountStatusEnum.Active;
    }
}
