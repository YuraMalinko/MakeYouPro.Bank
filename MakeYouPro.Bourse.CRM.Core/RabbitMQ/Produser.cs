using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MakeYouPro.Bourse.CRM.Core.RabbitMQ
{
    public class Produser<T> : IProduser<T>
    {
        private readonly string _exchangeName;

        private readonly string _queueName;

        private readonly IConnection _connection;

        private readonly IModel _channel;

        public Produser(IConnection connection, string exchangeName, string queueName)
        {
            _exchangeName = exchangeName;
            _queueName = queueName;
            _connection = connection;
            _channel = _connection.CreateModel();
            Initialize();
        }

        public void Publish(T value)
        {
            string message = JsonSerializer.Serialize(value);
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange: _exchangeName,
                                 routingKey: string.Empty,
                                 basicProperties: null,
                                 body: body);
        }

        private void Initialize()
        {
            _channel.ExchangeDeclare(exchange: _exchangeName, type: ExchangeType.Fanout);
            _channel.QueueDeclare(queue: _queueName);
            _channel.QueueBind(queue: _queueName,
                              exchange: _exchangeName,
                              routingKey: string.Empty);
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
