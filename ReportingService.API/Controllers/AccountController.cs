using Microsoft.AspNetCore.Mvc;
using ReportingService.Bll.IServices;
using ReportingService.Bll.Models.CRM;
using ReportingService.Bll.Services;

namespace ReportingService.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountServices _accountServices;
        public AccountController(IAccountServices accountServices) 
        {
            _accountServices = accountServices;
        }

        [HttpGet("Accounts")]
        public async Task<ActionResult<List<Account>>> GetAccountsByAmountOfTransactionsForPeriodAsync(int numberDays, int numberOfTransactions)
        {
            var result = await _accountServices.GetAccountsByAmountOfTransactionsForPeriodAsync(numberDays, numberOfTransactions);
            return Ok(result);
        }

        [HttpGet("Accounts/Birthday")]
        public async Task<ActionResult<List<Account>>> GetAccountsByBirthdayLeadsAsync(int numberDays)
        {
            var result = await _accountServices.GetAccountsByBirthdayLeadsAsync(numberDays);
            return Ok(result);
        }
    }
}
