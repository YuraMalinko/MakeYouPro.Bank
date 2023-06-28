using ReportingService.Dal.Models.TransactionStore;

namespace ReportingService.Dal.IRepository
{
    public interface ITransactionRepository
    {
        Task<TransactionEntity> CreateTransactionAsync(TransactionEntity transaction);

        Task<TransactionEntity> GetTransactionByIdOutsideAsync(int transactId);
    }
}