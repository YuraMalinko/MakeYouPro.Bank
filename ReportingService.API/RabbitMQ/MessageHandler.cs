using ReportingService.Dal.Models.CRM;
using ReportingService.Bll.IServices;

namespace ReportingService.Api.RabbitMQ
{
    internal class MessageHandler : IMessageHandler
    {
        private readonly IRecordingServices _recordingServices;
        public MessageHandler(IRecordingServices recordingServices) 
        { 
            _recordingServices = recordingServices;
        }

        public async void GetModelForRecordAsync(Object message, string routingKey)
        {
            if (routingKey == "Create")
            {
                CreateNewRecordAsync(message);
            }
            else if (routingKey == "Update")
            {
                UpdateRecordAsync(message);
            }
            else
            {
                throw new ArgumentException("Unnknow record type");
            }
        }

        private async void CreateNewRecordAsync(Object message)
        {
            if (message is LeadEntity lead)
            {
                await _recordingServices.CreateLeadInDatabaseAsync(lead);
            }
            else if (message is AccountEntity account)
            {
                await _recordingServices.CreateAccountInDatabaseAsync(account);
            }
            else
            {
                throw new ArgumentException("Unnknow record type");
            }
        }

        private async void UpdateRecordAsync(Object message)
        {
            //if (message is LeadEntity lead)
            //{
            //    await _recordingServices.CreateLeadInDatabaseAsync(lead);
            //}
            //else if (message is AccountEntity account)
            //{
            //    await _recordingServices.CreateAccountInDatabaseAsync(account);
            //}
            //else
            //{
            //    throw new ArgumentException("Unnknow record type");
            //}
        }
    }
}
