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
using FluentValidation;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using MakeYouPro.Bourse.CRM.Bll.Services;
using MakeYouPro.Bourse.CRM.Dal.Models;
using LogLevel = NLog.LogLevel;

namespace MakeYouPro.Bourse.CRM.Api.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AccountCotroller : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IValidator<AccountCreateRequest> _validatorCreate;
        public AccountCotroller(IAccountService accountService, IMapper mapper, ILogger nLogger, IValidator<AccountCreateRequest> validatorCreate)
        {
            _accountService = accountService;
            _mapper = mapper;
            _logger = nLogger;
            _validatorCreate = validatorCreate;
        }

        [HttpPost(Name = "CreateAccountAsync")]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.Conflict)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.PreconditionFailed)]

        public async Task<ActionResult<AccountResponse>> CreateAccountAsync([FromQuery] AccountCreateRequest account)
        {
            var validateAccount = await _validatorCreate.ValidateAsync(account);

            if (!validateAccount.IsValid)
            {
                foreach (var error in validateAccount.Errors)
                {
                    //_logger.Log(LogLevel.Debug,{ });
                    //_logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(CreateOrRecoverLeadAsync)}, 2 or more properties (email/phoneNumber/passportNumber) belong to different Leads in database.");
                    _logger.Log(LogLevel.Error, $"{nameof(LeadService)} {nameof(LeadEntity)}");
                }
                throw new AccountDataException(account, validateAccount.Errors[0].ErrorMessage);
            }

            var createAccount = await _accountService.CreateAccountAsync(_mapper.Map<Account>(account));

            return Created(new Uri("api/Account", UriKind.Relative), _mapper.Map<AccountResponse>(createAccount));
            //return Ok(_mapper.Map<AccountResponse>(createAccount));
        }
    }
}