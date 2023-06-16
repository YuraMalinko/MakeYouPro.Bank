using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReportingService.Api.MessageBroker.Configuration;
using ReportingService.Api.MessageBroker.Interfaces;

namespace ReportingService.Api.MessageBroker
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly RabbitMqSettings _settings;
        private readonly IHandlerFactory _handlerFactory;

        private IConnection _connection;
        private IModel _channel;

        public RabbitMqListener(RabbitMqSettings settings, IHandlerFactory handlerFactory)
        {
            _settings = settings;
            _handlerFactory = handlerFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken = default)
        {
            var factory = new ConnectionFactory() { HostName = _settings.HostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: _settings.Exchange, type: ExchangeType.Direct);
            var queueName = _channel.QueueDeclare().QueueName;
            foreach (var routing in _settings.Queues)
            {
                _channel.QueueBind(queue: queueName, exchange: _settings.Exchange, routingKey: routing);
            }

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var text = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;

                var handler = _handlerFactory.GetMessageHandler(routingKey);
                if (handler != null)
                {
                    handler.Handle(text);
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
