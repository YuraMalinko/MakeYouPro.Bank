using NLog;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MakeYouPro.Bourse.CRM.Core.RabbitMQ
{
    public abstract class ConsumerBase<T> : IConsumer<T>
    {
        private readonly IModel _channel;

        private readonly IConnection _connection;

        private readonly string _queueName;

        private readonly EventingBasicConsumer _consumer;

        private readonly ILogger _nLogger;

        //public ConsumerBase(string hostName, string queueName)

        public ConsumerBase(IConnection connection, string queueName, ILogger nLogger)
        {
            _queueName = queueName;
            _connection = connection;
            _nLogger = nLogger;
            _channel = _connection.CreateModel();
            _consumer = new EventingBasicConsumer(_channel);

            _consumer.Received += ParseAndHandleMessage;

            _channel.QueueDeclare(queue: _queueName);
            _channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: _consumer);
        }

        protected abstract void HandleMessage(T message);

        private void ParseAndHandleMessage(object? model, BasicDeliverEventArgs ea)
        {
            try
            {
                byte[] body = ea.Body.ToArray();
                var messageString = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<T>(messageString);

                if (message is not null)
                {
                    HandleMessage(message);
                }
            }
            catch (Exception ex)
            {
                _nLogger.Error(ex.Message + ex.StackTrace);
            }
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
