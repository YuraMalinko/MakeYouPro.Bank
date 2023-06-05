using RabbitMQ.Client;

namespace ReportingService.Api.RabbitMQ
{
    public interface IRabbitMqServicetest
    {
        void SendMessage(object obj);
        void SendMessage(string message);
    }
}
