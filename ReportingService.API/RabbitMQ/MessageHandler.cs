using ReportingService.Dal.Models.CRM;
using ReportingService.Bll.Services;

namespace RabPub
{
    internal class MessageHandler<T>
    {
        private readonly IRecordingServices _recordingServices;
        public MessageHandler(IRecordingServices recordingServices) 
        { 
            _recordingServices = recordingServices;
        }

        public async void GetModelForRecordAsync(T message)
        {
            var type = message.GetType();
            if (type == typeof(LeadEntity))
            {
                await _recordingServices.CreateLeadInDatabaseAsync(message as LeadEntity);
            }
            else if (type == typeof(AccountEntity))
            {

            }
            else
            {
                throw new ArgumentException("Unnknow record type");
            }
        }
    }
}
