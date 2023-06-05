using Microsoft.AspNetCore.Mvc;
using ReportingService.Api.RabbitMQ;

namespace ReportingService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMqController : ControllerBase
    {
        private readonly IRabbitMqServicetest _mqService;

        public RabbitMqController(IRabbitMqServicetest mqService)
        {
            _mqService = mqService;
        }

        [Route("[action]/{message}")]
        [HttpGet]
        public IActionResult SendMessage(string message)
        {
            _mqService.SendMessage(message);

            return Ok("Сообщение отправлено");
        }
    }
}
