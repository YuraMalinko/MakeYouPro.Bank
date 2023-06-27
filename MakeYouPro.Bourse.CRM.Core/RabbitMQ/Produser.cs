using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MakeYouPro.Bourse.CRM.Core.RabbitMQ
{
    public class Produser<T> : IProduser<T>
    {
        private string _exchangeName;

        private string _queueName;

        private IConnection _connection;

        private IModel _channel;

        public Produser(string hostName, string exchangeName, string queueName)
        {
            _exchangeName = exchangeName;
            _queueName = queueName;
            _connection = new ConnectionFactory { HostName = hostName }.CreateConnection();
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
    }
}
