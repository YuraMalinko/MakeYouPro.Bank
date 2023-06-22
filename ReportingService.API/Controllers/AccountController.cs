using Microsoft.AspNetCore.Mvc;

namespace ReportingService.Api.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
