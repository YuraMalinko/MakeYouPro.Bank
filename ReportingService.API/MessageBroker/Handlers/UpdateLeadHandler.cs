using Newtonsoft.Json;
using ReportingService.Api.MessageHandler;
using ReportingService.Bll.IServices;
using ReportingService.Bll.Services;
using ReportingService.Dal.Models.CRM;

namespace ReportingService.Api.MessageBroker.Handlers
{
    public class UpdateLeadHandler : IMessageHandler
    {
        private readonly IRecordingServices _recordingServices;
        public UpdateLeadHandler(IRecordingServices recordingServices)
        {
            _recordingServices = recordingServices;
        }
        public void Handle(string message)
        {
            var lead = JsonConvert.DeserializeObject<LeadEntity>(message);
            if (lead != null)
                Console.WriteLine("Update " + lead.Id);
        }
    }
}
