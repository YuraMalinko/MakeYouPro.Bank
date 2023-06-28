using AutoMapper;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Configurations.ISettings;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using MakeYouPro.Bourse.CRM.Dal.Models;
using ILogger = NLog.ILogger;

namespace MakeYouPro.Bourse.CRM.Bll.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILeadRepository _leadRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IAccountSetting _accountSetting;
        private readonly ICurrencySetting _currencySetting;

        public AccountService(ILeadRepository leadRepository,
            IAccountRepository accountRepository,
            IMapper mapper, ILogger logger,
            IAccountSetting accountSetting,
            ICurrencySetting currencySetting)
        {
            _leadRepository = leadRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _logger = logger;
            _accountSetting = accountSetting;
            _currencySetting = currencySetting;
        }

        public async Task<Account> CreateOrRestoreAccountAsync(Account account)
        {
            _logger.Debug($"Request execution process started.");
            var lead = _mapper.Map<Lead>(await _leadRepository.GetLeadByIdAsync(account.LeadId));

            if (!await CheckActiveStatusLead(lead))
            {
                var ex = new AccountArgumentException
                    ($"{nameof(Lead)} {lead} unsuitable status - {lead.Status}," +
                    $"for creating an account with a currency {account.Currency}." +
                    $"Need a status {LeadStatusEnum.Active}.");
                _logger.Warn(ex.Message);
                throw ex;
            }

            _logger.Debug($"Successfully checked the lead status - {lead.Status}.");

            if (!await CheckRightsCreateStandartAccount(lead, account.Currency))
            {
                var ex = new AccountArgumentException
                    ($"{nameof(Lead)} {lead} unsuitable role - {lead.Role}," +
                    $"for creating an account with a currency {account.Currency}." +
                    $"Need a Role - {LeadRoleEnum.StandardLead}.");
                _logger.Warn(ex.Message);
                throw ex;
            }
            else if (!await CheckRightsCreateVipAccount(lead, account.Currency))
            {
                var ex = new AccountArgumentException
                    ($"{nameof(Lead)} {lead} unsuitable role - {lead.Role}," +
                    $"for creating an account with a currency {account.Currency}." +
                    $"Need a Role - {LeadRoleEnum.StandardLead}.");
                _logger.Warn(ex.Message);
                throw ex;
            }

            _logger.Debug($"Successfully passed currency {account.Currency} " +
                $"and role lead {lead.Role} verification.");

            if (!await CheckAccountDublication(lead, account))
            {
                var ex = new AlreadyExistException($"{nameof(LeadEntity)} {lead} already has an active account with currency {account.Currency}");
                _logger.Warn(ex.Message);
                throw ex;
            }

            _logger.Debug($"Successfully passed verification for existing accounts with currency {account.Currency} of lead {lead}.");

            var deletedAccount = lead.Accounts.Find(a => a.Currency.Equals(account.Currency) && account.Status.Equals(AccountStatusEnum.Deleted));

            if (deletedAccount is not null)
            {
                _logger.Debug($"Previously deleted account {deletedAccount} with currency {account.Currency} was found, it will be restored.");
                var restoredAccount = await RestoreAccountAsync(deletedAccount);

                if (restoredAccount is not null)
                {
                    _logger.Info($"Account {restoredAccount} has been restored for Lead {lead}");
                }
                else
                {
                    var ex = new AccountUnknownException($"For some reason, the account {deletedAccount} belonging to lead {lead} Yura was not restored");
                    _logger.Error(ex.Message);
                    throw ex;
                }

                _logger.Debug($"The process of executing the request is completed");

                return restoredAccount!;
            }

            _logger.Debug($"Previously deleted accounts with the currency {account.Currency} were not found, a new one will be created.");
            var createAccount = await CreateAccountAsync(account);

            if (createAccount is not null)
            {
                _logger.Info($"Account {createAccount} has been restored for lead {lead}");
            }
            else
            {
                var ex = new AccountUnknownException($"For some reason, an account {account} for lead {lead} was not created");
                _logger.Warn(ex.Message);
                throw ex;
            }

            _logger.Debug($"The process of executing the request is completed");

            return createAccount!;
        }

        private async Task<Account> CreateAccountAsync(Account account)
        {
            account.Status = _accountSetting.StartAccountStatus;
            var createAccountEntity = await _accountRepository.CreateAccountAsync(_mapper.Map<AccountEntity>(account));
            var createAccount = _mapper.Map<Account>(createAccountEntity);

            return createAccount;
        }

        private async Task<Account> RestoreAccountAsync(Account account)
        {
            account.Status = AccountStatusEnum.Active;
            var restoredAccountEntity = await _accountRepository.ChangeAccountStatusAsync(_mapper.Map<AccountEntity>(account));
            var restoredAccount = _mapper.Map<Account>(restoredAccountEntity);

            return restoredAccount;
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            _logger.Debug($"Request execution process started.");
            var account = _mapper.Map<Account>(await _accountRepository.GetAccountAsync(accountId));

            if (account == null)
            {
                var ex = new NotFoundException(accountId, nameof(Lead));
                _logger.Warn(ex.Message);
                throw ex;
            }

            _logger.Debug($"Account {account} availability has been successfully verified.");

            if (account.Status != AccountStatusEnum.Active)
            {
                var ex = new AccountArgumentException($"{nameof(account)} {account} has already been deleted or deactive");
                _logger.Warn(ex.Message);
                throw ex;
            }

            _logger.Debug($"Account {account} status has been successfully verified.");

            //здесь место под запрос баланса

            if (account.Balance > 0)
            {
                var ex = new AccountArgumentException($"{nameof(account)} {account} has resources on the account balance - {account.Balance} {account.Currency}.");
                _logger.Warn(ex.Message);
                throw ex;
            }

            _logger.Debug($"The balance check has been successfully completed, account  deletion begins {account}.");
            bool result = false;
            account.Status = AccountStatusEnum.Deleted;
            var deletedAccount = await _accountRepository.ChangeAccountStatusAsync(_mapper.Map<AccountEntity>(account));

            if (deletedAccount is not null)
            {
                result = true;
                _logger.Info($"{nameof(deletedAccount)} {deletedAccount} is deleted.");
                //место для отправки в репорт сервис
            }
            else
            {
                var ex = new AccountUnknownException($"{nameof(account)} {account} for some reason it was not deleted.");
                _logger.Error(ex.Message);
                throw ex;
            }

            _logger.Debug($"The process of executing the request is completed.");

            return result;
        }

        public async Task<Account> ChangeAccountStatusAsync(Account updateAccount)
        {
            _logger.Debug($"Request execution process started.");
            var account = _mapper.Map<Account>(await _accountRepository.GetAccountAsync(updateAccount.Id));

            if (account == null)
            {
                var ex = new NotFoundException(updateAccount.Id, nameof(Lead));
                _logger.Warn(ex.Message);
                throw ex;
            }

            _logger.Debug($"Account {account} availability has been successfully verified.");

            if (account.Status == AccountStatusEnum.Deleted)
            {
                var ex = new AccountArgumentException($"{nameof(account)} {account} has already been deleted.");
                _logger.Warn(ex.Message);
                throw ex;
            }
            else if (updateAccount.Status == account.Status)
            {
                var ex = new AlreadyExistException(nameof(account.Status));
                _logger.Warn(ex.Message);
                throw ex;
            }

            _logger.Debug($"Account {account} status has been successfully verified.");

            var changeAccountEntiry = await _accountRepository.ChangeAccountStatusAsync(_mapper.Map<AccountEntity>(updateAccount));

            if (changeAccountEntiry != null)
            {
                _logger.Info($"The account has changed its status to {account.Status}.");
            }
            else
            {
                var ex = new AccountUnknownException($"For some reason, the account status change {account} failed.");
                _logger.Error(ex.Message);
                throw ex;
            }

            var changeAccount = _mapper.Map<Account>(changeAccountEntiry);

            _logger.Debug($"The process of executing the request is completed");

            return changeAccount;
        }

        public async Task<Account> UpdateAccountAsync(Account updateAccount)
        {
            _logger.Debug($"Request execution process started.");
            var account = _mapper.Map<Account>(await _accountRepository.GetAccountAsync(updateAccount.Id));

            if (account == null)
            {
                var ex = new NotFoundException(updateAccount.Id, nameof(Lead));
                _logger.Warn(ex.Message);
                throw ex;
            }

            _logger.Debug($"Account {account} availability has been successfully verified.");

            if (account.Status != AccountStatusEnum.Active)
            {
                var ex = new AccountArgumentException($"{nameof(account)} {account} has already been deleted or deactive");
                _logger.Warn(ex.Message);
                throw ex;
            }

            _logger.Debug($"Account {account} status has been successfully verified,update begins..");

            var changeAccountEntity = await _accountRepository.UpdateAccountAsync(_mapper.Map<AccountEntity>(updateAccount));

            if (changeAccountEntity != null)
            {
                _logger.Info($"The account {account} has changed its status to {updateAccount.Status}");
            }
            else
            {
                var ex = new AccountUnknownException($"For some reason, the account update {account} failed.");
                _logger.Error(ex.Message);
                throw ex;
            }

            var chandeAccount = _mapper.Map<Account>(changeAccountEntity);
            //место для запроса баланса
            _logger.Debug($"The process of executing the request is completed");

            return _mapper.Map<Account>(chandeAccount);
        }

        public async Task<Account> GetAccountAsync(int accountId)
        {
            var account = await _accountRepository.GetAccountAsync(accountId);

            if (account == null)
            {
                var ex = new FileNotFoundException($"There are no account matching by if {accountId}");
                _logger.Warn(ex.Message);
                throw ex;
            }

            _logger.Info($"Account information was uploaded by if {accountId}");

            return _mapper.Map<Account>(account);
        }

        public async Task<List<Account>> GetAccountsAsync(AccountFilter? filter)
        {
            _logger.Debug($"Request execution process started.");
            var accountsEntity = await _accountRepository.GetAccountsAsync(_mapper.Map<AccountFilterEntity>(filter));

            if (accountsEntity is not null)
            {
                _logger.Debug("The entire list of accounts has been unloaded from the database");
            }
            else
            {
                var ex = new FileNotFoundException($"For some reason, the list of accounts is empty");
                _logger.Warn(ex.Message);
                throw ex;
            }

            var accounts = _mapper.Map<List<Account>>(accountsEntity);

            if (filter is not null && (filter.FromBalace != null || filter.ToBalace != null))
            {
                foreach (var a in accounts)
                {
                    //здесь запросы баланса для всех аккаунтов
                }

                _logger.Debug("The balance is recorded for all accounts");

                if (filter.FromBalace != null)
                {
                    accounts.RemoveAll(a => a.Balance <= filter.FromBalace);
                }

                if (filter.ToBalace != null)
                {
                    accounts.RemoveAll(a => a.Balance >= filter.ToBalace);
                }

                _logger.Debug("Filter selection is performed");
            }

            if (accounts.Any())
            {
                _logger.Info($"The information about the accounts was uploaded by the filter");
            }
            else
            {
                var ex = new FileNotFoundException($"There are no accounts satisfying the filter.");
                _logger.Warn(ex.Message);
                throw ex;
            }

            _logger.Debug($"The process of executing the request is completed.");

            return accounts;
        }

        private async Task<bool> CheckActiveStatusLead(Lead lead)
        {
            bool result = true;

            if (lead.Status != LeadStatusEnum.Active)
            {
                result = false;
            }

            return result;
        }

        private async Task<bool> CheckRightsCreateVipAccount(Lead lead, string currency)
        {
            bool result = true;

            if (lead.Role == LeadRoleEnum.VipLead && !_currencySetting.CurrencyVip.Contains(currency))
            {
                result = false;
            }

            return result;
        }

        private async Task<bool> CheckRightsCreateStandartAccount(Lead lead, string currency)
        {
            bool result = true;

            if (lead.Role == LeadRoleEnum.StandardLead && !_currencySetting.CurrencyStandart.Contains(currency))
            {
                result = false;
            }

            return result;
        }

        private async Task<bool> CheckAccountDublication(Lead lead, Account account)
        {
            bool result = true;

            if (lead.Accounts.Exists(a => a.Currency == account.Currency && a.Status != AccountStatusEnum.Deleted))
            {
                result = false;
            }

            return result;
        }

        public async Task DeleteAccountByLeadIdAsync(int leadId)
        {
            await _accountRepository.DeleteAccountsByLeadIdAsync(leadId);
        }
    }
}