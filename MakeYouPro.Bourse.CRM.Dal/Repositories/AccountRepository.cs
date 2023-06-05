using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using MakeYouPro.Bourse.CRM.Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace MakeYouPro.Bourse.CRM.Dal.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private CRMContext _context;

        public AccountRepository(CRMContext context)
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

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            bool result = false;

            var account = await _context.Accounts
                .SingleOrDefaultAsync(a => a.Id == accountId && a.IsDeleted == false);

            if (account is not null)
            {
                account.IsDeleted = true;
                await _context.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<AccountEntity> ChangeAccountStatusAsync(AccountEntity accountUpdate)
        {
            var account = await _context.Accounts
                .SingleOrDefaultAsync(a => a.Id == accountUpdate.Id
                && a.Status != accountUpdate.Status
                && a.IsDeleted == false);

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
                && a.IsDeleted == false);

            if (account is not null)
            {
                account.Comment = accountUpdate.Comment;
                await _context.SaveChangesAsync();
            }

            return account!;
        }

        public async Task<AccountEntity> GetAnyAccountAsync(int accountId)
        {
            var result = (await _context.Accounts
                .Include(a => a.Lead)
                .SingleOrDefaultAsync(a => a.Id == accountId))!;
            return result;
        }


        public async Task<List<AccountEntity>> GetAccountsAsync(AccountFilterEntity? filter)
        {
            var accounts = await _context.Accounts
                .Include(a => a.Lead)
                .ToListAsync();

            if (filter is not null)
            {
                accounts.RemoveAll(a => a.DateCreate < filter.FromDateCreate && filter.FromDateCreate != null);
                accounts.RemoveAll(a => a.DateCreate.Date > filter.ToDateCreate && filter.ToDateCreate != null);
                accounts.RemoveAll(a => a.Balance < filter.FromBalace && filter.FromBalace != null);
                accounts.RemoveAll(a => a.Balance > filter.ToBalace && filter.ToBalace != null);
                accounts.RemoveAll(a => a.IsDeleted == filter.IsDeleted && filter.IsDeleted != null);
                accounts.RemoveAll(a => !filter.LeadsId.Contains(a.LeadId) && filter.LeadsId.Count != 0);
                accounts.RemoveAll(a => !filter.Currencies.Contains(a.Currency) && filter.Currencies.Count != 0);
                accounts.RemoveAll(a => filter.Statuses.Count != 0 && !filter.Statuses.Contains(a.Status));
            }

            return accounts;
        }
    }
}