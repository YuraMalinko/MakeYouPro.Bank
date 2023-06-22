using AutoMapper;
using ReportingService.Bll.Models.CRM;
using ReportingService.Dal.IRepository.CRM;

namespace ReportingService.Bll.Services
{
    public class AccountServices
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _map;

        public AccountServices(IAccountRepository accountRepository,IMapper map)
        {
            _accountRepository = accountRepository;
            _map = map;
        }

        public async Task<List<Account>> GetAccountsByBirthdayLeadsAsync(int numberDays)
        {
            var listAccount = await _accountRepository.GetAccountsByBirthdayLeadsAsync(numberDays);
            var result = _map.Map<List<Account>>(listAccount);
            return result;
        }

        public async Task<List<Account>> GetAccountsByAmountOfTransactionsForPeriodAsync(int numberDays, int numberOfTransactions)
        {
            var listAccount = await _accountRepository.GetAccountsByAmountOfTransactionsForPeriodAsync(numberDays, numberOfTransactions);
            var result = _map.Map<List<Account>>(listAccount);
            return result;
        }
    }
}
