using MakeYouPro.Bourse.CRM.Dal.Models;

namespace MakeYouPro.Bourse.CRM.Dal.IRepositories
{
    public interface IAccountRepository
    {
        Task<AccountEntity> CreateAccountAsync(AccountEntity account);

        Task<bool> DeleteAccountAsync(int accountId);

        Task<AccountEntity> ChangeAccountStatusAsync(AccountEntity accountUpdate);

        Task<AccountEntity> UpdateAccountAsync(AccountEntity accountUpdate);

        Task<AccountEntity> GetAnyAccountAsync(int accountId);

        Task<List<AccountEntity>> GetAccountsAsync(AccountFilterEntity? filter);

        Task DeleteAccountsByLeadIdAsync(int leadId);
    }
}