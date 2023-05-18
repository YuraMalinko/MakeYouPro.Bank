using Microsoft.AspNetCore.Mvc;

namespace MakeYouPro.Bank.CRM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetTest() 
        {
            TestBll bll = new TestBll();

            bll.GetTest();

            return Ok();
        }
    }
}
