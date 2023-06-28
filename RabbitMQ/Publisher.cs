using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace RabbitMQ
{
    public class Publisher
    {
        private IConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public Publisher(IConnectionFactory factory, IConnection connection, IModel channel)
        {
            _factory = factory;
            _connection = connection;
            _channel = channel;
        }

        public void SendMessage<T>(T value)
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };
            using (_connection = _factory.CreateConnection())
            using (_channel = _connection.CreateModel())
            {
                string queueName = "test";
                _channel.QueueDeclare(queue: queueName,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                var message = JsonSerializer.Serialize(value);
                var body = Encoding.UTF8.GetBytes(message);
                _channel.BasicPublish(exchange: "",
                                        routingKey: queueName,
                                        basicProperties: null,
                                        body: body);
                Console.WriteLine($"Sent {message}");
            }
        }
    }
}
