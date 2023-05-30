using AutoMapper;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Configurations.ISettings;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using MakeYouPro.Bourse.CRM.Dal.Models;
using ILogger = NLog.ILogger;
using LogLevel = NLog.LogLevel;

namespace MakeYouPro.Bourse.CRM.Bll.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILeadRepository _leadRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private IAccountSetting _accountSetting;
        private ICurrencySetting _currencySetting;

        public AccountService(ILeadRepository leadRepository, IAccountRepository accountRepository, IMapper mapper, ILogger logger,
            IAccountSetting accountSetting, ICurrencySetting currencySetting)
        {
            _leadRepository = leadRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _logger = logger;
            _accountSetting = accountSetting;
            _currencySetting = currencySetting;
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            var lead = await _leadRepository.GetLeadAsync(account.LeadId);

            if (lead == null)
            {
                _logger.Log(LogLevel.Error, $"{nameof(LeadEntity)} with id {account.LeadId} not found.");
                throw new NotFoundException(account.LeadId, nameof(LeadEntity));
            }

            else if (lead.IsDeleted is true)
            {
                var ex = new ArgumentException($"{nameof(LeadEntity)} with id {account.LeadId} is Deleted.");
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }
            else if (lead.Status != LeadStatusEnum.Active)
            {
                var ex = new ArgumentException($"{nameof(LeadEntity)} with id {account.LeadId} unsuitable status.");
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }

            if (lead.Role == LeadRoleEnum.VipLead && (_currencySetting.CurrencyStandart.Contains(account.Currency) || _currencySetting.CurrencyVip.Contains(account.Currency)))
            {
                var ex = new ArgumentException($"{nameof(LeadEntity)} with id {account.LeadId} unsuitable status.");
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }
            else if (lead.Role == LeadRoleEnum.VipLead && _currencySetting.CurrencyStandart.Contains(account.Currency))
            {
                var ex = new ArgumentException($"{nameof(LeadEntity)} with id {account.LeadId} unsuitable status.");
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }
            
            var identicalCurrencyAccount = lead.Accounts.FindAll(a => a.Currency == account.Currency).ToList();

            if (identicalCurrencyAccount.Count == 0)
            {
                if (identicalCurrencyAccount.Exists(a => a.IsDeleted is false))
                {

                }
            }
            
            account.Status = AccountStatusEnum.Active;
            account.Balance = 0;
            var newAccount = await _accountRepository.CreateAccountAsync(_mapper.Map<AccountEntity>(account));

            if (newAccount != null) { }

            return _mapper.Map<Account>(newAccount);
        }
    }
}
