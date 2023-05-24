
using MakeYouPro.Bource.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;

namespace MakeYouPro.Bourse.CRM.Dal.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public async Task<AccountEntity> CreateAccount(AccountEntity account)
        {
            return new AccountEntity();
        }
    }
}
