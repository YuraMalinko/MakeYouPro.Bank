using Newtonsoft.Json;
using ReportingService.Api.FinalRabbitMQ.MessageHandler;
using ReportingService.Dal.Models.CRM;

namespace ReportingService.Api.MessageBroker.Handlers
{
    public class CreateLeadHandler : IMessageHandler
    {
        public void Handle(string message)
        {
            var lead = JsonConvert.DeserializeObject<LeadEntity>(message);
            if (lead != null)
                Console.WriteLine("Creating " + lead.Id);
        }
    }
}
