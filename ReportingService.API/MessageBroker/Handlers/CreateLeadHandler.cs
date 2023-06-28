using Newtonsoft.Json;
using ReportingService.Api.MessageHandler;
using ReportingService.Bll.IServices;
using ReportingService.Dal.Models.CRM;

namespace ReportingService.Api.MessageBroker.Handlers
{
    public class CreateLeadHandler : IMessageHandler
    {
        private readonly IRecordingServices _recordingServices;

        public CreateLeadHandler(IRecordingServices recordingServices)
        {
            _recordingServices = recordingServices;
        }

        public void Handle(string message)
        {
            var lead = JsonConvert.DeserializeObject<LeadEntity>(message);
            if (lead != null)
                _recordingServices.CreateLeadInDatabaseAsync(lead);
        }
    }
}
