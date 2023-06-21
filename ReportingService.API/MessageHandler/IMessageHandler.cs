namespace ReportingService.Api.MessageHandler
{
    public interface IMessageHandler
    {
        void Handle(string message);
    }
}