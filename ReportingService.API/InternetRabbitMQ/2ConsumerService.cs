using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using ReportingService.Api.RabbitMQ;
using System.Text.Json;
using ReportingService.Dal.Models.CRM;

namespace ReportingService.Api.InternetRabbitMQ
{
    public class ConsumerService : IConsumerService, IDisposable
    {
        private readonly IModel _model;
        private readonly IConnection _connection;
        private IMessageHandler _messageHandler;

        public ConsumerService(IRabbitMqServices rabbitMqService, IMessageHandler messageHandler)
        {
            _connection = rabbitMqService.CreateChannel();
            _model = _connection.CreateModel();
            _model.QueueDeclare(_queueName, durable: true,
            exclusive: false, autoDelete: false,
            arguments: null);
            _messageHandler = messageHandler;
        }
        const string _queueName = "test";
        public async Task ReadMessgaes()
        {
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body.ToArray();
                var text = System.Text.Encoding.UTF8.GetString(body);
                Console.WriteLine(text);
                var message = JsonSerializer.Deserialize<LeadEntity>(text);
                //_messageHandler.GetModelForRecordAsync(message);
                await Task.CompletedTask;
                _model.BasicAck(ea.DeliveryTag, false);
            };
            _model.BasicConsume(_queueName, false, consumer);
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            if (_model.IsOpen)
                _model.Close();
            if (_connection.IsOpen)
                _connection.Close();
        }
    }
}
