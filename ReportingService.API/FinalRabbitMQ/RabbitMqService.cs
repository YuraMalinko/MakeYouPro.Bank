using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace ReportingService.Api.FinalRabbitMQ
{
    public class RabbitMqService : IRabbitMqService
    {
        public void SendMessage(string message)
        {
            SendMessageAsync(message);
        }

        public async Task SendMessageAsync(string message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    // Создание очереди
                    channel.ExchangeDeclare(exchange: "MakeYouPro", type: ExchangeType.Direct);

                    // Отправка задач в очередь

                    var serializedMessage = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(serializedMessage);
                    channel.BasicPublish(exchange: "MakeYouPro", routingKey: "Create", basicProperties: null, body: body);
                }
            }
        }
    }
}
