using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReportingService.Bll.Services;

namespace RabbitMQ
{
    public class Consumer
    {
        private IConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        private RecordingServices _record;

        public Consumer(IConnectionFactory factory, IConnection connection, IModel channel)
        {
            _factory = factory;
            _connection = connection;
            _channel = channel;
        }

        public T GetMessage<T>() where T : class 
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

                var consumer = new EventingBasicConsumer(_channel);
                T value = default;
                consumer.Received += (sender, args) =>
                {
                    var body = args.Body;
                    var message = Encoding.UTF8.GetString(body.ToArray());
                    value = JsonSerializer.Deserialize<T>(message);
                    //_record.CreateAnEntryInDatabaseAsync(value);
                };
                _channel.BasicConsume(queue: queueName,
                                        autoAck: true,
                                        consumer: consumer);
                return value;
            }
        }
    }
}
