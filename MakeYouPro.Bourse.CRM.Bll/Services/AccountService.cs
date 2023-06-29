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
        private readonly ICurrencySetting _currencySetting;

        public AccountService(ILeadRepository leadRepository,
            IAccountRepository accountRepository,
            IMapper mapper, ILogger logger,
            ICurrencySetting currencySetting)
        {
            _leadRepository = leadRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _logger = logger;
            _currencySetting = currencySetting;
        }

        public async Task<Account> CreateOrRestoreAccountAsync(Account account)
        {
            _logger.Info($"Request execution process started.");
            var lead = _mapper.Map<Lead>(await _leadRepository.GetLeadByIdAsync(account.LeadId));

            if (!await CheckActiveStatusLead(lead))
            {
                throw new AccountArgumentException
                    ($"{nameof(Lead)} {lead} unsuitable status - {lead.Status}," +
                    $"for creating an account with a currency {account.Currency}." +
                    $"Need a status {LeadStatusEnum.Active}.");
            }

            _logger.Info($"Successfully checked the lead status - {lead.Status}.");

            if (!await CheckRightsCreateStandartAccount(lead, account.Currency))
            {
                throw new AccountArgumentException
                    ($"{nameof(Lead)} {lead} unsuitable role - {lead.Role}," +
                    $"for creating an account with a currency {account.Currency}." +
                    $"Need a Role - {LeadRoleEnum.StandartLead}.");

            }
            else if (!await CheckRightsCreateVipAccount(lead, account.Currency))
            {
                throw new AccountArgumentException
                    ($"{nameof(Lead)} {lead} unsuitable role - {lead.Role}," +
                    $"for creating an account with a currency {account.Currency}." +
                    $"Need a Role - {LeadRoleEnum.StandartLead}.");
            }

            _logger.Info($"Successfully passed currency {account.Currency} " +
                $"and role lead {lead.Role} verification.");

            if (!await CheckAccountDublication(lead, account))
            {
                throw new AlreadyExistException($"{nameof(LeadEntity)} {lead} already has an active account with currency {account.Currency}");
            }

            _logger.Info($"Successfully passed verification for existing accounts with currency {account.Currency} of lead {lead}.");

            var deletedAccount = lead.Accounts.Find(a => a.Currency.Equals(account.Currency) && account.Status.Equals(AccountStatusEnum.Deleted));

            if (deletedAccount is not null)
            {
                _logger.Info($"Previously deleted account {deletedAccount} with currency {account.Currency} was found, it will be restored.");
                var restoredAccount = await RestoreAccountAsync(deletedAccount);

                if (restoredAccount is not null)
                {
                    _logger.Info($"Account {restoredAccount} has been restored for Lead {lead}");
                }
                else
                {
                    throw new AccountUnknownException($"For some reason, the account {deletedAccount} belonging to lead {lead} Yura was not restored");
                }

                _logger.Info($"The process of executing the request is completed");

                return restoredAccount!;
            }

            _logger.Info($"Previously deleted accounts with the currency {account.Currency} were not found, a new one will be created.");
            var createAccount = await CreateAccountAsync(account);

            if (createAccount is not null)
            {
                _logger.Info($"Account {createAccount} has been restored for lead {lead}");
            }
            else
            {
                throw new AccountUnknownException($"For some reason, an account {account} for lead {lead} was not created");
            }

            _logger.Info($"The process of executing the request is completed");

            return createAccount!;
        }

        private async Task<Account> CreateAccountAsync(Account account)
        {
            account.Status = AccountStatusEnum.Active;
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
            _logger.Info($"Request execution process started.");
            var account = _mapper.Map<Account>(await _accountRepository.GetAccountAsync(accountId));
            var lead = _mapper.Map<Lead>(await _leadRepository.GetLeadByIdAsync(account.LeadId));

            if (account == null)
            {
                throw new NotFoundException(accountId, nameof(Lead));
            }

            _logger.Info($"Account {account} availability has been successfully verified.");

            if (account.Status != AccountStatusEnum.Active)
            {
                throw new AccountArgumentException($"{nameof(account)} {account} has already been deleted or deactive");
            }

            _logger.Info($"Account {account} status has been successfully verified.");

            //здесь место под запрос баланса

            if (account.Balance > 0)
            {
                throw new AccountArgumentException($"{nameof(account)} {account} has resources on the account balance - {account.Balance} {account.Currency}.");
            }

            if (lead.Status != LeadStatusEnum.Deleted && account.Currency == _currencySetting.CurrencyDefault)
            {
                throw new AccountArgumentException($"the main currency account {account} cannot be deleted with the lead status {lead}");
            }

            _logger.Info($"The balance check has been successfully completed, account  deletion begins {account}.");
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
                throw new AccountUnknownException($"{nameof(account)} {account} for some reason it was not deleted.");
            }

            _logger.Info($"The process of executing the request is completed.");

            return result;
        }

        public async Task<Account> ChangeAccountStatusAsync(Account updateAccount)
        {
            _logger.Info($"Request execution process started.");
            var account = _mapper.Map<Account>(await _accountRepository.GetAccountAsync(updateAccount.Id));

            if (account == null)
            {
                throw new NotFoundException(updateAccount.Id, nameof(Lead));
            }

            _logger.Info($"Account {account} availability has been successfully verified.");

            if (account.Status == AccountStatusEnum.Deleted)
            {
                throw new AccountArgumentException($"{nameof(account)} {account} has already been deleted.");
            }
            else if (updateAccount.Status == account.Status)
            {
                throw new AlreadyExistException(nameof(account.Status));
            }

            _logger.Info($"Account {account} status has been successfully verified.");

            var changeAccountEntiry = await _accountRepository.ChangeAccountStatusAsync(_mapper.Map<AccountEntity>(updateAccount));

            if (changeAccountEntiry != null)
            {
                _logger.Info($"The account has changed its status to {account.Status}.");
            }
            else
            {
                throw new AccountUnknownException($"For some reason, the account status change {account} failed.");
            }

            var changeAccount = _mapper.Map<Account>(changeAccountEntiry);

            _logger.Info($"The process of executing the request is completed");

            return changeAccount;
        }

        public async Task<Account> UpdateAccountAsync(Account updateAccount)
        {
            _logger.Info($"Request execution process started.");
            var account = _mapper.Map<Account>(await _accountRepository.GetAccountAsync(updateAccount.Id));

            if (account == null)
            {
                throw new NotFoundException(updateAccount.Id, nameof(Lead));
            }

            _logger.Info($"Account {account} availability has been successfully verified.");

            if (account.Status != AccountStatusEnum.Active)
            {
                throw new AccountArgumentException($"{nameof(account)} {account} has already been deleted or deactive");
            }

            _logger.Info($"Account {account} status has been successfully verified,update begins..");

            var changeAccountEntity = await _accountRepository.UpdateAccountAsync(_mapper.Map<AccountEntity>(updateAccount));

            if (changeAccountEntity != null)
            {
                _logger.Info($"The account {account} has changed its status to {updateAccount.Status}");
            }
            else
            {
                throw new AccountUnknownException($"For some reason, the account update {account} failed.");
            }

            var chandeAccount = _mapper.Map<Account>(changeAccountEntity);
            _logger.Debug($"The process of executing the request is completed");

            return _mapper.Map<Account>(chandeAccount);
        }

        public async Task<Account> GetAccountAsync(int accountId)
        {
            var account = await _accountRepository.GetAccountAsync(accountId);

            if (account == null)
            {
                throw new FileNotFoundException($"There are no account matching by if {accountId}");
            }


            _logger.Info($"Account information was uploaded by if {accountId}");

            return _mapper.Map<Account>(account);
        }

        public async Task<List<Account>> GetAccountsAsync(AccountFilter? filter)
        {
            _logger.Info($"Request execution process started.");
            var accountsEntity = await _accountRepository.GetAccountsAsync(_mapper.Map<AccountFilterEntity>(filter));

            if (accountsEntity is not null)
            {
                _logger.Info("The entire list of accounts has been unloaded from the database");
            }
            else
            {
                throw new FileNotFoundException($"For some reason, the list of accounts is empty");
            }

            var accounts = _mapper.Map<List<Account>>(accountsEntity);

            
            if (accounts.Any())
            {
                _logger.Info($"The information about the accounts was uploaded by the filter");
            }
            else
            {
                throw new FileNotFoundException($"There are no accounts satisfying the filter.");
            }

            _logger.Info($"The process of executing the request is completed.");

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

            if (lead.Role == LeadRoleEnum.StandartLead && !_currencySetting.CurrencyStandart.Contains(currency))
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