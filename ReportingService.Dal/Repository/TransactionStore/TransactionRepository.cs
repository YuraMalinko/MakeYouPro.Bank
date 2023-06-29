using ReportingService.Dal.IRepository;
using ReportingService.Dal.Models.TransactionStore;

namespace ReportingService.Dal.Repository.TransactionStore
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly Context _context;

        public TransactionRepository(Context context)
        {
            _context = context;
        }

        public async Task CreateTransactionAsync(TransactionEntity transaction)
        {
            await _context.Transactions.AddAsync(transaction);
        }

        public Task<TransactionEntity> GetTransactionByIdOutsideAsync(int transactId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateTransactionAsync(TransactionEntity transaction)
        {
            throw new NotImplementedException();
        }
    }
}
