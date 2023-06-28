namespace ReportingService.Api.MessageBroker.Interfaces
{
    public interface ISerializer
    {
        public string Serialize(object obj);
    }
}
