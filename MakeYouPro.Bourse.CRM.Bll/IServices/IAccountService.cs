using MakeYouPro.Bourse.CRM.Bll.Models;

namespace MakeYouPro.Bourse.CRM.Bll.IServices
{
    public interface IAccountService
    {
        Task<Account> ChangeAccountStatusAsync(Account account);

        Task<Account> CreateAccountAsync(Account account);

        Task<bool> DeleteAccountAsync(int accountId);

        Task<Account> GetAccountAsync(int accountId);

        Task<List<Account>> GetAccountsAsync(AccountFilter filter);

        Task<Account> UpdateAccountAsync(Account account);

        Task DeleteAccountByLeadIdAsync(int leadId);
    }
}