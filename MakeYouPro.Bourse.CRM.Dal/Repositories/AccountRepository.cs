using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using MakeYouPro.Bourse.CRM.Dal.Models;
using Microsoft.EntityFrameworkCore;
using ILogger = NLog.ILogger;

namespace MakeYouPro.Bourse.CRM.Dal.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CRMContext _context;
        private readonly ILogger _logger;

        public AccountRepository(CRMContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<AccountEntity> CreateAccountAsync(AccountEntity account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();

            return (await _context.Accounts
                .AsNoTracking()
                .SingleOrDefaultAsync(a => a.Id == account.Id))!;
        }

        public async Task<AccountEntity> ChangeAccountStatusAsync(AccountEntity accountUpdate)
        {
            var account = await _context.Accounts
                .SingleOrDefaultAsync(a => a.Id == accountUpdate.Id
                && a.Status != accountUpdate.Status);

            if (account is not null)
            {
                account.Status = accountUpdate.Status;
                await _context.SaveChangesAsync();
            }

            return account!;
        }

        public async Task<AccountEntity> UpdateAccountAsync(AccountEntity accountUpdate)
        {
            var account = await _context.Accounts
                .SingleOrDefaultAsync(a => a.Id == accountUpdate.Id
                && a.Status != AccountStatusEnum.Deleted);

            if (account is not null)
            {
                account.Comment = accountUpdate.Comment;
                await _context.SaveChangesAsync();
            }

            return account!;
        }

        public async Task<AccountEntity> GetAccountAsync(int accountId)
        {
            var result = await _context.Accounts
                .Include(a => a.Lead)
                .AsNoTracking()
                .SingleOrDefaultAsync(a => a.Id == accountId);

            return result!;
        }

        public async Task<List<AccountEntity>> GetAccountsAsync()
        {
            var accounts = await _context.Accounts
                .Include(a => a.Lead).AsNoTracking().ToListAsync();

            return accounts;
        }
    }
}