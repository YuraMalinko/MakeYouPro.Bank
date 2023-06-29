using MakeYouPro.Bourse.LeadStatusUpdater.Service.Models;
using MakeYouPro.Bourse.LeadStatusUpdater.Service.RabbitMq;
using Newtonsoft.Json;

namespace MakeYouPro.Bourse.LeadStatusUpdater.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly RabbitMqPublisher _rabbitMqPublisher;
        private readonly string _routingKey;
        private readonly string _path;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, RabbitMqPublisher rabbitMqPublisher)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = new HttpClient();
            _rabbitMqPublisher = rabbitMqPublisher;
            _routingKey = _configuration.GetSection("AppSettings:RoutingKey").Value!;
            _path = _configuration.GetSection("AppSettings:PathToSettings").Value!;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime currentTime = DateTime.Now;

                if (currentTime.Hour == 2 && currentTime.Minute == 00)
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

                    HttpResponseMessage responseAccountsBirth = await GetResponseAccountsBirthAsync(settings);
                    HttpResponseMessage responseAccountsWithBigTransactions = await GetResponseAccountsWithALargeNumberOfTransactionsAsync(settings);
                    HttpResponseMessage responseAccountsWithFreshMoney = await GetResponseAccountsWithAccountsWithFreshMoneyAsync(settings);

                    List<LeadStatusUpdateModel> accountsBirth = new List<LeadStatusUpdateModel>();
                    List<LeadStatusUpdateModel> accountsWithBigTransactions = new List<LeadStatusUpdateModel>();
                    List<LeadStatusUpdateModel> accountsWithFreshMoney = new List<LeadStatusUpdateModel>();

                    if (responseAccountsBirth.IsSuccessStatusCode)
                    {
                        string dataAccountsBirth = await responseAccountsBirth.Content.ReadAsStringAsync();
                        accountsBirth = JsonConvert.DeserializeObject<List<LeadStatusUpdateModel>>(dataAccountsBirth);
                    }
                    else
                    {
                        _logger.LogError($"Ошибка при выполнении GET-запроса: {responseAccountsBirth.StatusCode}");
                    }

                    if (responseAccountsWithBigTransactions.IsSuccessStatusCode)
                    {
                        string dataAccountsWithBigTransactions = await responseAccountsWithBigTransactions.Content.ReadAsStringAsync();
                        accountsWithBigTransactions = JsonConvert.DeserializeObject<List<LeadStatusUpdateModel>>(dataAccountsWithBigTransactions);
                    }
                    else
                    {
                        _logger.LogError($"Ошибка при выполнении GET-запроса: {responseAccountsWithBigTransactions.StatusCode}");
                    }

                    if (responseAccountsWithFreshMoney.IsSuccessStatusCode)
                    {
                        string dataAccountsWithFreshMoney = await responseAccountsWithFreshMoney.Content.ReadAsStringAsync();
                        accountsWithFreshMoney = JsonConvert.DeserializeObject<List<LeadStatusUpdateModel>>(dataAccountsWithFreshMoney);
                    }
                    else
                    {
                        _logger.LogError($"Ошибка при выполнении GET-запроса: {responseAccountsWithFreshMoney.StatusCode}");
                    }

                    List<LeadStatusUpdateModel> accountsToPublish = await MergeListsAndRemoveDuplicatesAsync(accountsBirth, accountsWithBigTransactions, accountsWithFreshMoney);

                    try
                    {
                        await _rabbitMqPublisher.PublishMessageAsync(accountsToPublish, _routingKey);
                        _logger.LogInformation("Аккаунты Лидов для обновления статуса на Vip успешно отправлены в очередь");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Ошибка во время отправки Лидов в очередь: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка во время выполнения ProcessDataAsync: {ex.Message}");
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

        private async Task<HttpResponseMessage> GetResponseAccountsBirthAsync(Settings settings)
        {
            return await _httpClient.GetAsync($"/Account/Accounts/Birthday?numberDays={settings.PeriodOfBirthdayVIPInDays}");
        }

        private async Task<HttpResponseMessage> GetResponseAccountsWithALargeNumberOfTransactionsAsync(Settings settings)
        {
            return await _httpClient.GetAsync($"/Account/Accounts?numberDays={settings.PeriodOfTransactionsInDays}&numberOfTransactions={settings.CountOfTransactions}");
        }

        private async Task<HttpResponseMessage> GetResponseAccountsWithAccountsWithFreshMoneyAsync(Settings settings)
        {
            return await _httpClient.GetAsync($"/Account/Accounts?numberDays={settings.PeriodOfFreshMoneyInDays}&countOfFreshMoneyInRUB={settings.CountOfFreshMoneyInRUB}");
        }

        public static async Task<List<T>> MergeListsAndRemoveDuplicatesAsync<T>(List<T> list1, List<T> list2, List<T> list3)
        {
            List<T> resultList = await Task.Run(() => list1.Union(list2).Union(list3).ToList());
            return resultList;
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