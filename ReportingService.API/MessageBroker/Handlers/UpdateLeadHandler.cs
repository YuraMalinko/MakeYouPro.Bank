using Newtonsoft.Json;
using ReportingService.Api.FinalRabbitMQ.MessageHandler;
using ReportingService.Dal.Models.CRM;

namespace ReportingService.Api.MessageBroker.Handlers
{
    public class UpdateLeadHandler : IMessageHandler
    {
        public void GetModelForRecordAsync(object message, string routingKey)
        {
            throw new NotImplementedException();
        }

        public void Handle(string message)
        {
            var lead = JsonConvert.DeserializeObject<LeadEntity>(message);
            if (lead != null)
                Console.WriteLine("Update " + lead.Id);
        }
    }
}
