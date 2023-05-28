using MakeYouPro.Bourse.CRM.Dal.Models;

namespace MakeYouPro.Bourse.CRM.Dal.IRepositories
{
    public interface IAccountRepository
    {
        Task<AccountEntity> CreateAccountAsync(AccountEntity account);
    }
}
