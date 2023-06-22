namespace MakeYouPro.Bourse.CRM.Core.Clients.TransactionService
{
    public interface ITransactionServiceClient
    {
        Task<decimal> GetAccountBalanceAsync(int accountId);
    }
}
