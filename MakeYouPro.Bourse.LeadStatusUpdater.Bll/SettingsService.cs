using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace MakeYouPro.Bourse.LeadStatusUpdater.Bll
{
    public class SettingsService : ISettingsService
    {
        private readonly IConfiguration _configuration;

        public SettingsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Settings СhangeSettings(Settings settings)
        {
            string _path = _configuration.GetSection("AppSettings:PathToSettings").Value!;

            using (StreamWriter StreamWriter = new StreamWriter(_path))
            {
                string JsonTablesLis = JsonSerializer.Serialize(settings);
                StreamWriter.WriteLine(JsonTablesLis);
                return settings;
            }
        }

        public Settings GetSettings()
        {
            string _path = _configuration.GetSection("AppSettings:PathToSettings").Value!;

            using (StreamReader StreamWriter = new StreamReader(_path))
            {
                var result = new Settings();
                string jsn = StreamWriter.ReadLine();
                result = JsonSerializer.Deserialize<Settings>(jsn);
                return result;
            }

        }
    }
}
