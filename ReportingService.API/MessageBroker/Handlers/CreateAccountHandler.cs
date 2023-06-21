using Newtonsoft.Json;
using ReportingService.Api.MessageHandler;
using ReportingService.Bll.IServices;
using ReportingService.Dal.Models.CRM;

namespace ReportingService.Api.MessageBroker.Handlers
{
    public class CreateAccountHandler : IMessageHandler
    {
        private readonly IRecordingServices _recordingServices;

        public CreateAccountHandler(IRecordingServices recordingServices)
        {
            _recordingServices = recordingServices;
        }

        public void Handle(string message)
        {
            var account = JsonConvert.DeserializeObject<AccountEntity>(message);
            if (account != null) 
            _recordingServices.CreateAccountInDatabaseAsync(account);
        }
    }
}
