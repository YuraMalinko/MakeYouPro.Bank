namespace MakeYouPro.Bourse.LeadStatusUpdater.Service.RabbitMq.Interfaces
{
    public interface ISerializer
    {
        public string Serialize(object obj);
    }
}
