using MakeYouPro.Bourse.CRM.Core.Enums;

namespace MakeYouPro.Bourse.CRM.Core.Configurations.ISettings
{
    public interface IAccountSetting
    {
        AccountStatusEnum StartAccountStatus { get; set; }
    }
}
