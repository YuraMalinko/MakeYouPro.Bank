using RabbitMQ.Client;

namespace ReportingService.Api.InternetRabbitMQ
{
    public interface IRabbitMqServices
    {
        IConnection CreateChannel();
    }
}
