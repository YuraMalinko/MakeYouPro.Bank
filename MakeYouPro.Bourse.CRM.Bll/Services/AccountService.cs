using AutoMapper;
using MakeYouPro.Bank.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Core.Clients.AuthService;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using NLog;

namespace MakeYouPro.Bourse.CRM.Bll.Services
{
    public class AccountService : IAccountService
    {

        private readonly IAccountRepository _accountRepository;

        private readonly IMapper _mapper;

        private readonly ILogger _logger;

        public AccountService(IAccountRepository accountRepository, IMapper mapper, ILogger nLogger)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _logger = nLogger;
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            return new Account();
        }

        public async Task DeleteAccountByLeadIdAsync(int leadId)
        {
            await _accountRepository.DeleteAccountsByLeadIdAsync(leadId);
        }
    }
}
