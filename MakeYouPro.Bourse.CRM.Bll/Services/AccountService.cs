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
            var lead = _mapper.Map<Lead>(await _leadRepository.GetLeadAsync(account.LeadId));

            Exception ex = null;

            ex = await CheckLeadCondition(lead, account.LeadId);
            if (ex != null)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }
            ex = await CheckRightsLead(lead, account);
            if (ex != null)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }
            ex = await CheckAccountDublication(lead, account);
            if (ex != null)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }

            account.Status = _accountSetting.StartAccountStatus;
            account.Balance = _accountSetting.StartBalance;
            var result = await _accountRepository.CreateAccountAsync(_mapper.Map<AccountEntity>(account));

            if (result != null)
            {
                _logger.Log(LogLevel.Info, $"Account with currency {result.Currency} created for lead {result.LeadId}");
            }

            return _mapper.Map<Account>(result);
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            var account = _mapper.Map<Account>(await _accountRepository.GetAnyAccountAsync(accountId));

            Exception ex = null;

            ex = await CheckAccountCondition(accountId, account);
            if (ex != null)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }

            if (account.Balance > 0)
            {
                ex = new AccountDataException("the account balance is not zero");
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }

            var result = await _accountRepository.DeleteAccountAsync(accountId);

            if (result is true)
            {
                _logger.Log(LogLevel.Info, $"Account {accountId} is deleted");
            }

            return result;
        }

        public async Task<Account> ChangeAccountStatusAsync(Account account)
        {
            var oldAccount = _mapper.Map<Account>(await _accountRepository.GetAnyAccountAsync(account.Id));

            Exception ex = null;

            ex = await CheckAccountCondition(account.Id, oldAccount);
            if (ex != null)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }

            if (account.Status == oldAccount.Status)
            {
                ex = new AlreadyExistException(nameof(account.Status));
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }

            var result = await _accountRepository.ChangeAccountStatusAsync(_mapper.Map<AccountEntity>(account));

            if (result != null)
            {
                _logger.Log(LogLevel.Info, $"The account has changed its status to {account.Status.ToString()}");
            }

            return _mapper.Map<Account>(result);
        }

        public async Task<Account> UpdateAccountAsync(Account account)
        {
            var oldAccount = _mapper.Map<Account>(await _accountRepository.GetAnyAccountAsync(account.Id));

            Exception ex = null;

            ex = await CheckAccountCondition(account.Id, oldAccount);
            if (ex != null)
            {
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }

            var result = await _accountRepository.UpdateAccountAsync(_mapper.Map<AccountEntity>(account));

            if (result != null)
            {
                _logger.Log(LogLevel.Info, $"The account has changed its status to {account.Status.ToString()}");
            }

            return _mapper.Map<Account>(result);
        }

        public async Task<Account> GetAccountAsync(int accountId)
        {
            var account = await _accountRepository.GetAnyAccountAsync(accountId);

            if (account == null)
            {
                var ex = new FileNotFoundException($"There are no account matching by if {accountId}");
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }

            _logger.Log(LogLevel.Info, $"Account information was uploaded by if {accountId}");

            return _mapper.Map<Account>(account);
        }

        public async Task<List<Account>> GetAccountsAsync(AccountFilter filter)
        {
            var accounts = await _accountRepository.GetAccountsAsync(_mapper.Map<AccountFilterEntity>(filter));

            if (accounts == null)
            {
                var ex = new FileNotFoundException("There are no accounts matching the filter");
                _logger.Log(LogLevel.Error, ex.Message);
                throw ex;
            }

            _logger.Log(LogLevel.Info, $"The information about the accounts was uploaded by the filter");

            return _mapper.Map<List<Account>>(accounts);
        }

        private async Task<Exception> CheckAccountCondition(int accountId, Account account)
        {
            Exception ex = null;

            if (account == null)
            {
                ex = new NotFoundException(accountId, $"{nameof(account)} with id {accountId} not found.");
            }
            else if (account.IsDeleted is true)
            {
                ex = new ArgumentException($"{nameof(account)} with id {account.LeadId} is Deleted.");
            }

            return ex;
        }

        private async Task<Exception> CheckRightsLead(Lead lead, Account account)
        {
            Exception ex = null;

            if (lead.Role == LeadRoleEnum.VipLead && (!_currencySetting.CurrencyStandart.Contains(account.Currency) && !_currencySetting.CurrencyVip.Contains(account.Currency)))
            {
                ex = new ArgumentException($"{nameof(LeadEntity)} with id {account.LeadId} unsuitable role for creating an account with a currency .");
            }
            else if (lead.Role == LeadRoleEnum.StandardLead && !_currencySetting.CurrencyStandart.Contains(account.Currency))
            {
                ex = new ArgumentException($"{nameof(LeadEntity)} with id {account.LeadId} unsuitable role for creating an account with a currency.");
            }
            return ex;
        }

        private async Task<Exception> CheckLeadCondition(Lead lead, int LeadId)
        {
            Exception ex = null;

            if (lead == null)
            {
                ex = new NotFoundException(LeadId, $"{nameof(Lead)} with id {LeadId} not found.");
            }
            else if (lead.IsDeleted is true)
            {
                ex = new ArgumentException($"{nameof(Lead)} with id {LeadId} is Deleted.");
            }
            else if (lead.Status != LeadStatusEnum.Active)
            {
                ex = new ArgumentException($"{nameof(Lead)} with id {LeadId} unsuitable status.");
            }
            return ex;
        }

        private async Task<Exception> CheckAccountDublication(Lead lead, Account account)
        {
            Exception ex = null;

            var identicalCurrencyAccount = lead.Accounts.FindAll(a => a.Currency == account.Currency).ToList();

            if (identicalCurrencyAccount.Count != 0)
            {
                var identicalAccount = identicalCurrencyAccount.Find(a => a.IsDeleted is false);
                if (identicalAccount is not null)
                {
                    ex = new AlreadyExistException($"{nameof(LeadEntity)} with id {account.LeadId} already has an active account with currency {account.Currency}");
                }
            }
            return ex;
        }

        public async Task DeleteAccountByLeadIdAsync(int leadId)
        {
            await _accountRepository.DeleteAccountsByLeadIdAsync(leadId);
        }
    }
}