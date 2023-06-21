namespace ReportingService.Api.MessageBroker.Configuration
{
    public class RouteServiceSettings
    {
        public string CreateLeadRoutingKey { get; set; }

        public string UpdateLeadRoutingKey { get; set; }

        public string CreateAccountRoutingKey { get; set; }

        public string UpdateAccountRoutingKey { get; set; }
    }
}
