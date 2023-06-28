using System.Text;
using RabbitMQ.Client;
using ReportingService.Api.MessageBroker.Configuration;
using ReportingService.Api.MessageBroker.Interfaces;

namespace ReportingService.Api.MessageBroker
{
    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        private readonly RabbitMqSettings _settings;
        private readonly ISerializer _serializer;
        private readonly ILogger _logger;

        public RabbitMqPublisher(RabbitMqSettings settings,
                               ISerializer serializer,
                               ILogger<RabbitMqPublisher> logger)
        {
            _settings = settings;
            _serializer = serializer;
            _logger = logger;
        }

        public async Task PublishMessageAsync<T>(T message, string routingKey)
        {
            _logger.LogInformation($"Send message command received. Message: {message}");

            var factory = new ConnectionFactory() { HostName = _settings.HostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    // Создание exchange
                    channel.ExchangeDeclare(exchange: _settings.Exchange, type: ExchangeType.Direct);

                    // Отправка задач в очередь
                    var messageString = _serializer.Serialize(message);
                    var body = Encoding.UTF8.GetBytes(messageString);

                    // Пишем в очередь
                    channel.BasicPublish(exchange: _settings.Exchange, routingKey: routingKey, basicProperties: null, body: body);

                    _logger.LogInformation($"Message published");
                }
            }
        }
    }
}
