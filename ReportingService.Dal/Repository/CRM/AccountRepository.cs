using CoreRS.Enums;
using Microsoft.EntityFrameworkCore;
using ReportingService.Dal.IRepository.CRM;
using ReportingService.Dal.Models.CRM;

namespace ReportingService.Dal.Repository.CRM
{
    public class AccountRepository : IAccountRepository
    {
        private Context _context;

        public AccountRepository(Context context)
        {
            _context = context;
        }

        public async Task<AccountEntity> CreateAccountAsync(AccountEntity account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();

            return (await _context.Accounts
                .SingleOrDefaultAsync(a => a.Id == account.Id))!;
        }

        public async Task UpdateAccountAsync(AccountEntity accountUpdate)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.Id == accountUpdate.Id
            && a.Status != AccountStatusEnum.Deleted);

            if (account is not null)
            {
                account.Comment = accountUpdate.Comment;
                account.Status = accountUpdate.Status;
                account.IsDeleted = accountUpdate.IsDeleted;
                account.DateCreate = accountUpdate.DateCreate;
                account.Balance = accountUpdate.Balance;
                account.Currency = accountUpdate.Currency;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<AccountEntity>> GetAccountsByBirthdayLeadsAsync(int numberDays)
        {
            DateOnly startDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-numberDays));
            DateOnly nowDate = DateOnly.FromDateTime(DateTime.Now);
            var listAccounts = await _context.Accounts.Where(d => d.Leads.Birthday >= startDate && d.Leads.Birthday <= nowDate)
                                                      .ToListAsync();
            return listAccounts;
        }

        public async Task<List<AccountEntity>> GetAccountsByAmountOfTransactionsForPeriodAsync(int numberDays, int numberOfTransactions)
        {
            var startDate = DateTime.Now.AddDays(-numberDays);

            var listAccounts = await _context.Accounts
                .Where(a => a.Transactions
                .Count(t => t.Type != TransactionType.Withdraw && t.DataTime >= startDate && t.DataTime <= DateTime.Now) >= numberOfTransactions)
                .ToListAsync();

            return listAccounts;
        }

        public async Task<List<AccountEntity>> GetAccountsByAmountDifference(int numberDays, int sum)
        {
            var startDate = DateTime.Now.AddDays(-numberDays);

            var x = _context.Transactions.Where(t => t.DataTime >= startDate && t.DataTime <= DateTime.Now)
                .GroupBy(i => i.AccountId);
            //.Where(s => s.Where(t => t.Type == TransactionType.Deposit).Sum(t => t.Amount));
            return new List<AccountEntity>();
        }
    }
}
