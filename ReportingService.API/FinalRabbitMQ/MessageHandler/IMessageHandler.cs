namespace ReportingService.Api.FinalRabbitMQ.MessageHandler
{
    public interface IMessageHandler
    {       
        void Handle(string message);
    }
}