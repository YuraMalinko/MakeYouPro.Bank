using MakeYouPro.Bourse.CRM.Bll.Models;

namespace MakeYouPro.Bourse.CRM.Bll.IServices
{
    public interface IAccountService
    {
        Task<Account> CreateOrRestoreAccountAsync(Account account);

        Task<bool> DeleteAccountAsync(int accountId);

        Task DeleteAccountByLeadIdAsync(int leadId);

        Task<Account> ChangeAccountStatusAsync(Account account);

        Task<Account> UpdateAccountAsync(Account account);

        Task<Account> GetAccountAsync(int accountId);

        Task<List<Account>> GetAccountsAsync(AccountFilter filter);
    }
}