using ReportingService.Dal.Models.TransactionStore;

namespace ReportingService.Dal.IRepository
{
    public interface ITransactionRepository
    {
        Task CreateTransactionAsync(TransactionEntity transaction);

        Task UpdateTransactionAsync(TransactionEntity transaction);

        Task<TransactionEntity> GetTransactionByIdOutsideAsync(int transactId);
    }
}