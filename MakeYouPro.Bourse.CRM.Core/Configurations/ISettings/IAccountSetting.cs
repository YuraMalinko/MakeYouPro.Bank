using MakeYouPro.Bourse.CRM.Core.Enums;

namespace MakeYouPro.Bourse.CRM.Core.Configurations.ISettings
{
    public interface IAccountSetting
    {
        decimal StartBalance { get; set; }

        AccountStatusEnum StartAccountStatus { get; set; }
    }
}
