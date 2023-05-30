using AutoMapper;
using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Account.Request;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using MakeYouPro.Bourse.CRM.Models.Account.Response;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using ILogger = NLog.ILogger;
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
                string exMessage = "";
                foreach (var error in validateAccount.Errors)
                {
                    _logger.Log(LogLevel.Error, error.ErrorMessage);
                    exMessage += $"{error.ErrorMessage} |   ";
                }
                throw new AccountDataException(account, exMessage);
            }
            var createAccount = await _accountService.CreateAccountAsync(_mapper.Map<Account>(account));

            return Created(new Uri("api/Account", UriKind.Relative), _mapper.Map<AccountResponse>(createAccount));
        }
    }
}