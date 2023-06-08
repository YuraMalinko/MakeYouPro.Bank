namespace ReportingService.Api.FinalRabbitMQ
{
    public interface IRabbitMqService
    {
        Task SendMessageAsync(string message);
    }
}
