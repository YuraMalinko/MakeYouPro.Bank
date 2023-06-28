using Newtonsoft.Json;
using ReportingService.Api.MessageHandler;
using ReportingService.Bll.IServices;
using ReportingService.Dal.Models.CRM;

namespace ReportingService.Api.MessageBroker.Handlers
{
    public class UpdateAccountHandler : IMessageHandler
    {
        private readonly IRecordingServices _recordingServices;

        public UpdateAccountHandler(IRecordingServices recordingServices)
        {
            _recordingServices = recordingServices;
        }

        public void Handle(string message)
        {
            var account = JsonConvert.DeserializeObject<AccountEntity>(message);
            if (account != null)

                _recordingServices.UpdateAccountInDatebaseAync(account);
        }
    }
}
