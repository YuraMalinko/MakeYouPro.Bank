using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReportingService.Dal.Repository.CRM;

namespace ReportingService.Api.RabbitMQ
{

    public class RabbitMqListener : BackgroundService
    {
        private IConnection _connectionFactory;
        private IModel _channel;

        public RabbitMqListener()
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            var factory = new ConnectionFactory { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var _channel = connection.CreateModel();

            _channel.QueueDeclare("demo-queue", durable: true,
            exclusive: true, autoDelete: false,
            arguments: null);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += (_, args) =>
            {
                var message = JsonSerializer.Deserialize<>(args.Body.ToArray());
                Console.WriteLine(message);
                _channel.BasicAck(args.DeliveryTag, multiple: false);
                return Task.CompletedTask;
            };
            _channel.BasicConsume(consumer, queue: "test");
        }
    }
}

