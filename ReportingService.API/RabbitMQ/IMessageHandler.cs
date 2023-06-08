namespace ReportingService.Api.RabbitMQ
{
    public interface IMessageHandler
    {
        void GetModelForRecordAsync(Object message, string routingKey);
    }
}