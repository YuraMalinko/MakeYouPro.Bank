using Microsoft.EntityFrameworkCore;
using NLog;
using ReportingService.Dal.IRepository;
using ReportingService.Dal.Models.TransactionStore;

namespace ReportingService.Dal.Repository.TransactionStore
{
    {
        private readonly Context _context;
        private readonly ILogger _logger;

        {
            _context = context;
            _logger = log;
        }

        public async Task<TransactionEntity> CreateTransactionAsync(TransactionEntity transaction)
        {
            await _context.Transactions.AddAsync(transaction);

            return await _context.Transactions
        }

        {
            {
            }
        }
    }
}
