using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReportingService.Api.RabbitMQ;

namespace ReportingService.Api.FinalRabbitMQ
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IMessageHandler _handler;

        public RabbitMqListener(IMessageHandler handler)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _handler = handler;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel.ExchangeDeclare(exchange: "MakeYouPro", type: ExchangeType.Direct);
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: "MakeYouPro", routingKey: "Create");
            _channel.QueueBind(queue: queueName, exchange: "MakeYouPro", routingKey: "Update");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var text = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey; 
                var message = JsonConvert.DeserializeObject<Object>(text);
                _handler.GetModelForRecordAsync(message, routingKey);
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
