using RabbitMQ.Client;

namespace ReportingService.Api.InternetRabbitMQ
{
    public interface IRabbitMqService
    {
        IConnection CreateChannel();
    }
}
