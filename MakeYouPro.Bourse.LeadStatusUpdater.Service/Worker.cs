using System.Text.Json;

namespace MakeYouPro.Bourse.LeadStatusUpdater.Service
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                DateTime currentTime = DateTime.Now;

                if (currentTime.Hour == 20)
                {
                    var settings = new Settings();
                    settings = GetSettings();
                    Console.WriteLine(settings.PeriodOfTransactionsInDays);
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
                    HttpResponseMessage response = await _httpClient.GetAsync("https://api.example.com/data");

                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Ошибка при выполнении GET-запроса: {response.StatusCode}");
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка во время выполнения GET-запроса: {ex.Message}");
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