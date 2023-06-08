using System.Text;
using RabbitMQ.Client;

namespace ReportingService.Api.FinalRabbitMQ
{
    public class RabbitMqService : IRabbitMqService
    {
        public async Task SendMessageAsync(string message, string routingKey)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    // Создание очереди
                    channel.ExchangeDeclare(exchange: "MakeYouPro", type: ExchangeType.Direct);

                    // Отправка задач в очередь
                    if (routingKey == "Create")
                    {
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "MakeYouPro", routingKey: "Create", basicProperties: null, body: body);
                    }
                    else if (routingKey == "Update")
                    {
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "MakeYouPro", routingKey: "Update", basicProperties: null, body: body);
                    }
                }
            }
        }
    }
}
