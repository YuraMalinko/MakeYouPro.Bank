using MakeYouPro.Bourse.CRM.Dal.Models;

namespace MakeYouPro.Bourse.CRM.Dal.IRepositories
{
    public interface IAccountRepository
    {
        Task<AccountEntity> CreateAccountAsync(AccountEntity account);

        Task<AccountEntity> ChangeAccountStatusAsync(AccountEntity accountUpdate);

        Task<AccountEntity> UpdateAccountAsync(AccountEntity accountUpdate);

        Task<AccountEntity> GetAccountAsync(int accountId);

        Task<List<AccountEntity>> GetAnyAccountsAsync(AccountFilterEntity account);
    }
}