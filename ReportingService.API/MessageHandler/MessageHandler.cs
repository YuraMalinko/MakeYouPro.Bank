using ReportingService.Dal.Models.CRM;
using ReportingService.Bll.IServices;
using Newtonsoft.Json;
using ReportingService.Api.MessageBroker.Interfaces;

namespace ReportingService.Api.MessageHandler
{
    internal class MessageHandler : IMessageHandler
    {
        private readonly IRecordingServices _recordingServices;
        private readonly IRabbitMqPublisher _rabbitMqService;
        private readonly string create = "Create";
        private readonly string update = "Update";
        public MessageHandler(IRecordingServices recordingServices, IRabbitMqPublisher rabbitMqService)
        {
            _recordingServices = recordingServices;
            _rabbitMqService = rabbitMqService;
        }

        public async Task SendMessageForCreateAsync(object message)
        {
            var text = JsonConvert.SerializeObject(message);
            //await _rabbitMqService.SendMessageAsync(text, create);
        }

        public async Task SendMessageForUpdateAsync(object message)
        {
            var text = JsonConvert.SerializeObject(message);
            //await _rabbitMqService.SendMessageAsync(text, update);
        }

        public async void GetModelForRecordAsync(object message, string routingKey)
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

        private async void CreateNewRecordAsync(object message)
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

        private async void UpdateRecordAsync(object message)
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

        public void Handle(string message)
        {
            throw new NotImplementedException();
        }
    }
}
