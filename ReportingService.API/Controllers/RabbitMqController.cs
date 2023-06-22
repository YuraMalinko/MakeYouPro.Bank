using Microsoft.AspNetCore.Mvc;
using ReportingService.Api.MessageBroker.Configuration;
using ReportingService.Api.MessageBroker.Interfaces;
using ReportingService.Dal.Models.CRM;

namespace ReportingService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RabbitMqController : ControllerBase
    {
        private readonly IRabbitMqPublisher _mqPublisher;
        private readonly RouteServiceSettings _settings;

        public RabbitMqController(IRabbitMqPublisher mqPublisher, RouteServiceSettings settings)
        {
            _mqPublisher = mqPublisher;
            _settings = settings;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(LeadEntity message)
        {
            await _mqPublisher.PublishMessageAsync(message, _settings.CreateLeadRoutingKey);
            return Ok("Сообщение отправлено");
        }

        [HttpGet]
        public async Task<IActionResult> ReadAllMessages()
        {
            return Ok();
        }
    }
}
