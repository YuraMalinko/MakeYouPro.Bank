namespace MakeYouPro.Bourse.LeadStatusUpdater.Bll
{
    public interface ISettingsService
    {
        Settings GetSettings();
        Settings СhangeSettings(Settings settings);
    }
}
