using MakeYouPro.Bourse.CRM.Core.Clients.TransactionService.Models;

namespace MakeYouPro.Bourse.CRM.Core.Clients.TransactionService
{
    public interface ITransactionServiceClient
    {
        Task<decimal> GetAccountBalanceAsync(int accountId);

        Task<int> CreateWithdrawTransactionAsync(WithdrawRequest transaction);

        Task<int> CreateDepositTransactionAsync(DepositRequest transaction);
    }
}
