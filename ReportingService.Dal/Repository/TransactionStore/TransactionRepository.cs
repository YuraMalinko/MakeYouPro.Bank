using Microsoft.EntityFrameworkCore;
using NLog;
using ReportingService.Dal.Models.TransactionStore;

namespace ReportingService.Dal.Repository.TransactionStore
{
    public class TransactionRepository
    {
        private readonly Context _context;
        private readonly ILogger _logger;

        public TransactionRepository(Context context, ILogger log)
        {
            _context = context;
            _logger = log;
        }

        public async Task<TransactionEntity> CreateTransactionAsync(TransactionEntity transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return await _context.Transactions
                .Include(k => k.Account)
                .SingleAsync(k => k.Id == transaction.Id);
        }
    }
}
