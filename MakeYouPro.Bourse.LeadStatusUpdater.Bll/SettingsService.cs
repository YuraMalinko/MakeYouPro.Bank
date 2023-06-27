using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MakeYouPro.Bourse.LeadStatusUpdater.Bll
{
    public class SettingsService : ISettingsService
    {
        //private readonly IMapper _mapper;
        //private readonly IHostingEnvironment env;

        //public SettingsService(IMapper mapper)
        //{
        //    _mapper = mapper;
        //    _userRepository = userRepository;
        //}
        public Settings GetSettings()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            //C:\Users\vladm\OneDrive\Документы\GitHub\MakeYouPro.Bank\MakeYouPro.Bourse.LeadStatusUpdater.Service\appsettings.json
            var result = new Settings();

            result.PeriodOfTransactionsInDays = Convert.ToInt32(config.GetSection("RequestsSettings:PeriodOfTransactionsInDays").Value);
            result.CountOfTransactions = Convert.ToInt32(config.GetSection("RequestsSettings:CountOfTransactions").Value);
            result.PeriodOfFreshMoneyInDays = Convert.ToInt32(config.GetSection("RequestsSettings:PeriodOfFreshMoneyInDays").Value);
            result.CountOfFreshMoneyInRUB = Convert.ToInt32(config.GetSection("RequestsSettings:CountOfFreshMoneyInRUB").Value);
            result.PeriodOfBirthdayVIPInDays = Convert.ToInt32(config.GetSection("RequestsSettings:PeriodOfBirthdayVIPInDays").Value);

            return result;
        }
        //public async Task<Settings> СhangeSettingsAsync(Settings settings)
        //{
        //    return settings;
        //}
    }
}
