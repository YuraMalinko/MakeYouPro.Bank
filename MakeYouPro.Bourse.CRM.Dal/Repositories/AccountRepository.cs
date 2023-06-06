
using MakeYouPro.Bource.CRM.Dal;
using MakeYouPro.Bource.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using MakeYouPro.Bourse.CRM.Dal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NLog;

namespace MakeYouPro.Bourse.CRM.Dal.Repositories
{
    public class AccountRepository : IAccountRepository
    {

        private static CRMContext _context;

        private readonly ILogger _logger;

        public AccountRepository(CRMContext context, ILogger nLogger)
        {
            _context = context;
            _logger = nLogger;
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
                accounts.RemoveAll(a => a.IsDeleted == filter.AccountIsDeleted && filter.AccountIsDeleted != null);
                accounts.RemoveAll(a => !filter.LeadsId.Contains(a.LeadId) && filter.LeadsId.Count != 0);
                accounts.RemoveAll(a => !filter.Currencies.Contains(a.Currency) && filter.Currencies.Count != 0);
                accounts.RemoveAll(a => filter.Statuses.Count != 0 && !filter.Statuses.Contains(a.Status));
            }

            return accounts;
        }

        public async Task DeleteAccountsByLeadIdAsync(int leadId)
        {
            var leadDb = await _context.Leads.SingleOrDefaultAsync(l => l.Id == leadId);
            if (leadDb == null)
            {
                _logger.Log(LogLevel.Debug, $"{nameof(LeadEntity)} with id {leadId} not found.");
                throw new NotFoundException(leadId, nameof(LeadEntity));
            }
            else
            {
                await _context.Accounts.Where(a => a.LeadId == leadId).ForEachAsync(a => a.IsDeleted = true);
                await _context.SaveChangesAsync();
            }
        }
    }
}