using AutoMapper;
using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Account.Request;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Enums;
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
        private readonly IValidator<AccountFilterRequest> _validatorFilter;

        public AccountCotroller(IAccountService accountService, IMapper mapper, ILogger nLogger, IValidator<AccountCreateRequest> validatorCreate, IValidator<AccountFilterRequest> validatorFilter)
        {
            _accountService = accountService;
            _mapper = mapper;
            _logger = nLogger;
            _validatorCreate = validatorCreate;
            _validatorFilter = validatorFilter;
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
                throw new AccountDataException(exMessage);
            }

            var createAccount = await _accountService.CreateAccountAsync(_mapper.Map<Account>(account));

            return Created(new Uri("api/Account", UriKind.Relative), _mapper.Map<AccountResponse>(createAccount));
        }

        [HttpDelete(Name = "DeletedAccountAsync")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.PreconditionFailed)]
        public async Task<ActionResult<bool>> DeletedAccountAsync(int accountId)
        {
            if (accountId <=0)
            {
                var ex = new ArgumentException("The account ID cannot be equal to or less than zero");
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }
            else
            {
                var result = await _accountService.DeleteAccountAsync(accountId);

                return Ok(result);
            }
        }

        [HttpPatch("Reactivation", Name = "ReactivationAccount")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.PreconditionFailed)]
        public async Task<ActionResult<AccountResponse>> ReactivationAccount(int accountId)
        {
            if (accountId <= 0)
            {
                var ex = new ArgumentException("The account ID cannot be equal to or less than zero");
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }
            else
            {
                var account = new Account()
                {
                    Id = accountId,
                    Status = AccountStatusEnum.Active
                };
                var result = await _accountService.ChangeAccountStatusAsync(account);

                return Ok(_mapper.Map<AccountResponse>(result));
            }
        }

        [HttpPatch("Deactivation", Name = "DeactivationAccount")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.PreconditionFailed)]
        public async Task<ActionResult<AccountResponse>> DeactivationAccount(int accountId)
        {
            if (accountId <= 0)
            {
                var ex = new ArgumentException("The account ID cannot be equal to or less than zero");
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }
            else
            {
                var account = new Account()
                {
                    Id = accountId,
                    Status = AccountStatusEnum.Deactive
                };
                var result = await _accountService.ChangeAccountStatusAsync(account);

                return Ok(_mapper.Map<AccountResponse>(result));
            }
        }

        [HttpPatch("Update", Name = "UpdateAccount")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.PreconditionFailed)]
        public async Task<ActionResult<AccountResponse>> UpdateAccount([FromQuery] AccountUpdateRequest account)
        {
            if (account.Id <= 0)
            {
                var ex = new AccountDataException("The account ID cannot be equal to or less than zero");
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }
            else
            {
                var result = await _accountService.UpdateAccountAsync(_mapper.Map<Account>(account));
                return Ok(_mapper.Map<AccountResponse>(result));
            }
        }

        [HttpGet("{accountId}", Name = "GetAccount")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.PreconditionFailed)]
        public async Task<ActionResult<AccountResponse>> GetAccount(int accountId)
        {
            if (accountId <= 0)
            {
                var ex = new AccountDataException("The account ID cannot be equal to or less than zero");
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }
            else
            {
                var result = await _accountService.GetAccountAsync(accountId);

                return Ok(_mapper.Map<AccountResponse>(result));
            }
        }

        [HttpGet(Name = "GetAccounts")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        [SwaggerResponse((int)HttpStatusCode.PreconditionFailed)]
        public async Task<ActionResult<List<AccountResponse>>> GetAccounts([FromQuery] AccountFilterRequest filter)
        {
            var validateAccount = await _validatorFilter.ValidateAsync(filter);

            if (!validateAccount.IsValid)
            {
                string exMessage = "";
                foreach (var error in validateAccount.Errors)
                {
                    _logger.Log(LogLevel.Error, error.ErrorMessage);
                    exMessage += $"{error.ErrorMessage} |   ";
                }
                throw new AccountDataException(exMessage);
            }

            var accounts = await _accountService.GetAccountsAsync(_mapper.Map<AccountFilter>(filter));

            return Ok(_mapper.Map<List<AccountResponse>>(accounts));
        }
    }
}