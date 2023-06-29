using System.Text;
using RabbitMQ.Client;
using WebAPIClient;

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

        public void SendMessage ()
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };
            using (_connection = _factory.CreateConnection())
            using (_channel = _connection.CreateModel())
            {
                string queueName = "Rates";
                _channel.QueueDeclare(queue: queueName,
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                var message = "RateStorage.jsonNew";
                var body = Encoding.UTF8.GetBytes(RateStorage.jsonNew);
                _channel.BasicPublish(exchange: "",
                                        routingKey: queueName,
                                        basicProperties: null,
                                        body: body);
                Console.WriteLine($"Sent {message}");
            }
        }
    }
}
