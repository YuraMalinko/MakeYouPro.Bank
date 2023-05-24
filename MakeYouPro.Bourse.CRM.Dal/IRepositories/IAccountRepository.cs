using MakeYouPro.Bource.CRM.Dal.Models;

namespace MakeYouPro.Bourse.CRM.Dal.IRepositories
{
    public interface IAccountRepository
    {
        Task<AccountEntity> CreateAccount(AccountEntity account);
    }
}
