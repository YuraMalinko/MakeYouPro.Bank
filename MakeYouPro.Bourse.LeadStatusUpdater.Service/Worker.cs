using MakeYouPro.Bourse.LeadStatusUpdater.Service.Models;
using MakeYouPro.Bourse.LeadStatusUpdater.Service.RabbitMq;
using Newtonsoft.Json;
using ILogger = NLog.ILogger;
using LogLevel = NLog.LogLevel;

namespace MakeYouPro.Bourse.LeadStatusUpdater.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly RabbitMqPublisher rabbitMqPublisher;
        private readonly string _routingKey;
        private readonly string _path;

        public Worker(ILogger nLogger, IConfiguration configuration)
        {
            _logger = nLogger;
            _configuration = configuration;
            _httpClient = new HttpClient();
            _routingKey = _configuration.GetSection("AppSettings:RoutingKey").Value!;
            _path = _configuration.GetSection("AppSettings:PathToSettings").Value!;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime currentTime = DateTime.Now;

                if (currentTime.Hour == 16 && currentTime.Minute == 12)
                {
                    await ProcessDataAsync();
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task ProcessDataAsync()
        {
            try
            {
                using (HttpClient client = _httpClient)
                {
                    var settings = new Settings();
                    settings = GetSettings();
                    Console.WriteLine(settings.PeriodOfTransactionsInDays);

                    HttpResponseMessage responseAccountsBirth = await _httpClient.GetAsync($"/Account/Accounts/Birthday?numberDays={settings.PeriodOfBirthdayVIPInDays}");
                    HttpResponseMessage responseAccountsWithBigTransactions = await _httpClient.GetAsync($"/Account/Accounts?numberDays={settings.PeriodOfTransactionsInDays}&numberOfTransactions={settings.CountOfTransactions}");

                    if (responseAccountsBirth.IsSuccessStatusCode & responseAccountsWithBigTransactions.IsSuccessStatusCode)
                    {
                        string dataAccountsBirth = await responseAccountsBirth.Content.ReadAsStringAsync();
                        string dataAccountsWithBigTransactions = await responseAccountsWithBigTransactions.Content.ReadAsStringAsync();

                        List<LeadStatusUpdateModel> accountsBirth = JsonConvert.DeserializeObject<List<LeadStatusUpdateModel>>(dataAccountsBirth);
                        List<LeadStatusUpdateModel> accountsWithBigTransactions = JsonConvert.DeserializeObject<List<LeadStatusUpdateModel>>(dataAccountsWithBigTransactions);

                        var accountsToPublish = accountsBirth.Union(accountsWithBigTransactions).ToList();

                        rabbitMqPublisher.PublishMessageAsync(accountsToPublish, _routingKey);
                        _logger.Log(LogLevel.Info, "Аккаунты Лидов для обновления статуса на Vip отправлены в очередь");
                    }
                    else
                    {
                        if (!responseAccountsBirth.IsSuccessStatusCode)
                            _logger.Log(LogLevel.Error, $"Ошибка при выполнении GET-запроса: {responseAccountsBirth.StatusCode}");
                        if (!responseAccountsWithBigTransactions.IsSuccessStatusCode)
                            _logger.Log(LogLevel.Error, $"Ошибка при выполнении GET-запроса: {responseAccountsWithBigTransactions.StatusCode}");
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"Ошибка при выполнении GET-запроса: {ex.Message}");
            }
        }

        private Settings GetSettings()
        {
            using (StreamReader StreamWriter = new StreamReader(_path))
            {
                var result = new Settings();
                string jsn = StreamWriter.ReadLine();
                result = System.Text.Json.JsonSerializer.Deserialize<Settings>(jsn);
                return result;
            }

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Info, $"{nameof(Worker)}: Starting");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Info, $"{nameof(Worker)}: Stopping");
            return base.StopAsync(cancellationToken);
        }
    }
}