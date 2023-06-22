using Microsoft.Extensions.Configuration;

namespace MakeYouPro.Bourse.LeadStatusUpdater.ServiceSettingsForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private SettingsModel GetSettings()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var settings = new SettingsModel() { };

            settings.PeriodOfTransactionsInDays = Convert.ToInt32(config.GetSection("RequestsSettings:PeriodOfTransactionsInDays").Value);
            settings.CountOfTransactions = Convert.ToInt32(config.GetSection("RequestsSettings:CountOfTransactions").Value);
            settings.PeriodOfFreshMoneyInDays = Convert.ToInt32(config.GetSection("RequestsSettings:PeriodOfFreshMoneyInDays").Value);
            settings.CountOfFreshMoneyInRUB = Convert.ToInt32(config.GetSection("RequestsSettings:CountOfFreshMoneyInRUB").Value);
            settings.PeriodOfBirthdayVIPInDays = Convert.ToInt32(config.GetSection("RequestsSettings:PeriodOfBirthdayVIPInDays").Value);

            return settings;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var settings = GetSettings();
            MessageBox.Show($"{settings.PeriodOfTransactionsInDays}");
        }
    }
}