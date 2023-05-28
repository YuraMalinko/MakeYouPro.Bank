using AutoMapper;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Models.Account.Response;
using MakeYouPro.Bourse.CRM.Api.Models.Account.Request;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using ILogger = NLog.ILogger;
using LogManager = NLog.LogManager;

namespace MakeYouPro.Bourse.CRM.Api.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AccountCotroller : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccountCotroller(IAccountService accountService, IMapper mapper, ILogger nLogger)
        {
            _accountService = accountService;
            _mapper = mapper;
            _logger = nLogger;
        }

        [HttpPost(Name = "CreateAccountAsync")]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.Conflict)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<AccountResponse>> CreateAccountAsync([FromQuery] AccountCreateRequest account)
        {
            if (account == null)
            {

            }
            else if (account.Currency.IsNullOrEmpty())
            {

            }
            else if (account.Comment is not null && account.Comment.IsNullOrEmpty())
            {

            }

            var createAccount = await _accountService.CreateAccountAsync(_mapper.Map<Account>(account));

            return Created(new Uri("api/Account", UriKind.Relative), _mapper.Map<AccountResponse>(createAccount));
            //return Ok(_mapper.Map<AccountResponse>(createAccount));
        }
    }
}