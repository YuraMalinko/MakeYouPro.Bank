using AutoMapper;
using Castle.Core.Logging;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using ILogger = NLog.ILogger;
using LogManager = NLog.LogManager;

namespace MakeYouPro.Bourse.CRM.Bll.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILeadRepository _leadRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private List<string> _currencyStandart = new List<string>() { "RUB", "USD", "EUR" };
        private List<string> _currencyVip = new List<string>() { "JPY", "CNY", "RSD", "BGN", "ARS" };

        public AccountService(ILeadRepository leadRepository, IAccountRepository accountRepository, IMapper mapper, ILogger logger)
        {
            _leadRepository = leadRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            var lead = await _leadRepository.GetLeadAsync(account.LeadId);

            if (lead == null) { }
            else if (lead.IsDeleted is true) { }
            else if (lead.Status != LeadStatusEnum.Active) { }
            else if (lead.Role != LeadRoleEnum.StandardLead && _currencyVip.Contains(account.Currency)) { }

            var identicalCurrencyAccount = lead.Accounts.FindAll(a=>a.Currency == account.Currency).ToList();

            if (identicalCurrencyAccount.Count==0)
            {
                if (identicalCurrencyAccount.Exists(a=>a.IsDeleted is false))
                {

                }
            }

            account.Status = AccountStatusEnum.Active;
            account.Balance = 0;
            var newAccount = await _accountRepository.CreateAccountAsync( _mapper.Map<AccountEntity>(account));
            
            if(newAccount != null) { }

            return _mapper.Map<Account>(newAccount);
        }
    }
}
