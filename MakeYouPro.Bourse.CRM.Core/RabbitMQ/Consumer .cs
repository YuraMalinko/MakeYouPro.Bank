using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MakeYouPro.Bourse.CRM.Core.RabbitMQ
{
    public class Consumer<T> : IConsumer<T>
    {
        private readonly IModel _channel;

        private readonly IConnection _connection;

        private readonly string _queueName;

        private readonly EventingBasicConsumer _consumer;

        public Consumer(string hostName, string queueName)
        {
            _queueName = queueName;
            _connection = new ConnectionFactory { HostName = hostName }.CreateConnection();
            _channel = _connection.CreateModel();
            _consumer = new EventingBasicConsumer(_channel);

            _consumer.Received += HandleMessage;
            _channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: _consumer);
        }

        private void HandleMessage(object? model, BasicDeliverEventArgs ea)
        {
            byte[] body = ea.Body.ToArray();
            var messageString = Encoding.UTF8.GetString(body);
            var message = JsonSerializer.Deserialize<T>(messageString);
        }

        public void Dispose()
        {
            _channel.Close();
            _channel.Dispose();
            _connection.Close();
            _connection.Dispose();
        }
    }
}
