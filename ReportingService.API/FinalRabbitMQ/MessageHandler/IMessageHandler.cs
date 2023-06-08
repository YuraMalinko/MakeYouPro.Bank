namespace ReportingService.Api.FinalRabbitMQ.MessageHandler
{
    public interface IMessageHandler
    {
        void GetModelForRecordAsync(object message, string routingKey);
    }
}