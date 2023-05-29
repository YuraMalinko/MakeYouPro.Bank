using MakeYouPro.Bank.CRM.Bll.Models;

namespace MakeYouPro.Bourse.CRM.Bll.IServices
{
    public interface IAccountService
    {
        Task<Account> CreateAccountAsync(Account account);
    }
}
