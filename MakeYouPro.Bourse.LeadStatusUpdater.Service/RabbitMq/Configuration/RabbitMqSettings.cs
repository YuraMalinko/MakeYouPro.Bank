namespace MakeYouPro.Bourse.LeadStatusUpdater.Service.RabbitMq.Configuration
{
    public class RabbitMqSettings
    {
        public string HostName { get; set; }

        public string Exchange { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string[] Queues { get; set; }
    }
}
