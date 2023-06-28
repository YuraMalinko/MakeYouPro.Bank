namespace ReportingService.Api.MessageBroker.Interfaces
{
    public interface IRabbitMqPublisher
    {
        Task PublishMessageAsync<T>(T message, string routingKey);
    }
}
