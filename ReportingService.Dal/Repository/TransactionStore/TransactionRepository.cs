using Microsoft.EntityFrameworkCore;

namespace ReportingService.Dal.Repository.TransactionStore
{
    public class TransactionRepository
    {
        private readonly Context _context;

        public TransactionRepository(Context context)
        {
            _context = context;
        }
        public async Task<int> CreateTransactionAsync(TransactionEntity transaction)
        {
            //хранимка на добавление в базу с установкой времени;
            var transactionId = await _context.Database.SqlQuery<int>($"EXEC AddTransaction {transaction.AccountId}, {transaction.Type}, {transaction.Amount}").ToListAsync();

            return transactionId[0];
        }
    }
}
