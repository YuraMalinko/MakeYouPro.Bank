using System.Text.Json;

namespace MakeYouPro.Bourse.LeadStatusUpdater.Bll
{
    public class SettingsService : ISettingsService
    {

        public Settings СhangeSettings(Settings settings)
        {
            using (StreamWriter StreamWriter = new StreamWriter("../MakeYouPro.Bourse.LeadStatusUpdater.Service/MakeYouPro.Bourse.LeadStatusUpdater.Service.Settings.Test.txt"))
            {
                string JsonTablesLis = JsonSerializer.Serialize(settings);
                StreamWriter.WriteLine(JsonTablesLis);
                return settings;
            }
        }

        public Settings GetSettings()
        {
            using (StreamReader StreamWriter = new StreamReader("../MakeYouPro.Bourse.LeadStatusUpdater.Service/MakeYouPro.Bourse.LeadStatusUpdater.Service.Settings.Test.txt"))
            {
                var result = new Settings();
                string jsn = StreamWriter.ReadLine();
                result = JsonSerializer.Deserialize<Settings>(jsn);
                return result;
            }
        }
    }
}
