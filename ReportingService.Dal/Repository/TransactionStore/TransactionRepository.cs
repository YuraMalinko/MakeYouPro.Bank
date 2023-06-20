using Microsoft.EntityFrameworkCore;
using ReportingService.Dal.Models.TransactionStore;

namespace ReportingService.Dal.Repository.TransactionStore
{
    public class TransactionRepository
    {
        private readonly Context _context;

        public TransactionRepository(Context context)
        {
            _context = context;
        }
        public async Task<TransactionEntity> CreateTransactionAsync(TransactionEntity transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            _context.SaveChanges();

            return await _context.Transactions
                .SingleAsync(l => l.Id == transaction.Id);
        }

        public async Task UpdateTransactionAsync(TransactionEntity transactionUpdate)
        {
            var transaction = await _context.Transactions.SingleOrDefaultAsync(t => t.Id == transactionUpdate.Id);
            if (transaction != null)
            {
                transaction.Type = transactionUpdate.Type;
                transaction.AccountId = transactionUpdate.AccountId;
                transaction.Amount = transactionUpdate.Amount;
                transaction.Time = transactionUpdate.Time;
                await _context.SaveChangesAsync();
            }
        }
    }
}
