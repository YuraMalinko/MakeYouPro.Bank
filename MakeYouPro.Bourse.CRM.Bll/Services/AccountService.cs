using MakeYouPro.Bank.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Bll.IServices;

namespace MakeYouPro.Bourse.CRM.Bll.Services
{
    public class AccountService : IAccountService
    {
        public async Task<Account> CreateAccountAsync(Account account)
        {
            return new Account();
        }
    }
}
