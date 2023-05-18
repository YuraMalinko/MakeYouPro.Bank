using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace RabbitMQ
{
    public class Publisher<T>
    {
        public T Value { get; set; }
        private ConnectionFactory factory;
        private IConnection connection;
        private IModel channel;

        public void SendMessege()
        {
            factory = new ConnectionFactory() { HostName = "localhost" };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            string queueName = "test";
            var message = JsonSerializer.Serialize(Value);
            var body = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(exchange: "",
                                routingKey: queueName,
                                basicProperties: null,
                                body: body);
            Console.WriteLine($" [x] Sent {message}");

            channel.Close();
            connection.Close();
        }
    }
}
