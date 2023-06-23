

using MakeYouPro.Bourse.CRM.Bll.Models;

namespace MakeYouPro.Bourse.CRM.Bll.IServices
{
    public interface ITransactionService
    {
        Task<decimal> GetAccountBalanceAsync(int accountId);

        Task<int> CreateWithdrawAsync(Transaction transaction);

        Task<int> CreateDepositAsync(Transaction transaction);
    }
}
