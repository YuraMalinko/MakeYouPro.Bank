namespace MakeYouPro.Bourse.LeadStatusUpdater.Service.RabbitMq.Interfaces
{
    public interface IRabbitMqPublisher
    {
        Task PublishMessageAsync<T>(T message, string routingKey);
    }
}
