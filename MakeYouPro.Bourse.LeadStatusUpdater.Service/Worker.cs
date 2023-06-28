using System.Text.Json;
using MakeYouPro.Bourse.CRM.Models.Account.Response;
using Newtonsoft.Json;
using ReportingService.Api.MessageBroker;

namespace MakeYouPro.Bourse.LeadStatusUpdater.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly RabbitMqPublisher rabbitMqPublisher;
        private readonly string _routingKey;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = new HttpClient();
            _routingKey = "update-lead-status-on-vip";
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime currentTime = DateTime.Now;

                if (currentTime.Hour == 20)
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

                    //HttpResponseMessage response = accountsBirth.Union(accountsWithBigTransactions).ToList();
                    if (responseAccountsBirth.IsSuccessStatusCode & responseAccountsWithBigTransactions.IsSuccessStatusCode)
                    {
                        string dataAccountsBirth = await responseAccountsBirth.Content.ReadAsStringAsync();
                        string dataAccountsWithBigTransactions = await responseAccountsWithBigTransactions.Content.ReadAsStringAsync();

                        List<AccountResponse> accountsBirth = JsonConvert.DeserializeObject<List<AccountResponse>>(dataAccountsBirth);
                        List<AccountResponse> accountsWithBigTransactions = JsonConvert.DeserializeObject<List<AccountResponse>>(dataAccountsWithBigTransactions);

                        var accountsToPublish = accountsBirth.Union(accountsWithBigTransactions).ToList();

                        rabbitMqPublisher.PublishMessageAsync(accountsToPublish, _routingKey);
                        _logger.LogInformation("Аккаунты Лидов для обновления статуса на Vip отправлены в очередь");
                    }
                    else
                    {
                        if(!responseAccountsBirth.IsSuccessStatusCode)
                            _logger.LogError($"Ошибка при выполнении GET-запроса: {responseAccountsBirth.StatusCode}");
                        if (!responseAccountsWithBigTransactions.IsSuccessStatusCode)
                            _logger.LogError($"Ошибка при выполнении GET-запроса: {responseAccountsWithBigTransactions.StatusCode}");
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка во время выполнения GET-запроса: {ex.Message}");
            }
        }

        private Settings GetSettings()
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

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(Worker)}: Start");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(Worker)} stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}