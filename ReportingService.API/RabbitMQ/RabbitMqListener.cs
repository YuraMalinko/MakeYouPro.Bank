using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ReportingService.Api.RabbitMQ
{

    public class RabbitMqListener : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private IMessageHandler _messageHandler;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(30);

        public RabbitMqListener(IMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var _connection = factory.CreateConnection();
            var _channel = _connection.CreateModel();
            _channel.QueueDeclare("test", durable: true,
            exclusive: false, autoDelete: false,
            arguments: null);
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += (_, args) =>
            {
                var message = JsonSerializer.Deserialize<Object>(args.Body.ToArray());
                _messageHandler.GetModelForRecordAsync(message);
                _channel.BasicAck(args.DeliveryTag, multiple: false);
                return Task.CompletedTask;
            };
            _channel.BasicConsume(consumer, queue: "test");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(_interval, stoppingToken);
            }
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}

