namespace ReportingService.Api.InternetRabbitMQ
{
    public class ConsumerHostedService : BackgroundService
    {
        private readonly IConsumerService _consumerService;

        public ConsumerHostedService(IConsumerService consumerService)
        {
            _consumerService = consumerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                await _consumerService.ReadMessgaes();
                Thread.Sleep(1000);
            }
            while (!stoppingToken.IsCancellationRequested);
        }
    }
}
