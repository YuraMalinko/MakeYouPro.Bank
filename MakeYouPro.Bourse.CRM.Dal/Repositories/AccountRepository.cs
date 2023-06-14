using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
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

        public async Task<List<AccountEntity>> GetAccountsAsync(AccountFilterEntity? filter)
        {
            IQueryable<AccountEntity> accounts = _context.Accounts;

            if (filter!.FromDateCreate is not null)
            {
                accounts = accounts.Where(a => a.DateCreate >= filter.FromDateCreate);
                _logger.Debug( $"The data is sorted by filter - FromDateCreate.");
            }

            if (filter.ToDateCreate is not null)
            {
                accounts = accounts.Where(a => a.DateCreate.Date <= filter.ToDateCreate);
                _logger.Debug( $"The data is sorted by filter - ToDateCreate.");
            }

            if (filter.LeadsId!.Any())
            {
                accounts = accounts.Where(a => filter.LeadsId!.Contains(a.LeadId));
                _logger.Debug($"The data is sorted by filter - LeadsId.");
            }

            if (filter.Currencies!.Any())
            {
                accounts = accounts.Where(a => filter.Currencies!.Contains(a.Currency));
                _logger.Debug( $"The data is sorted by filter - Currencies.");
            }

            if (filter.Statuses!.Any())
            {
                accounts = accounts.Where(a => filter.Statuses!.Contains(a.Status));
                _logger.Debug( $"The data is sorted by filter - Statuses.");
            }

            var result = await accounts.Include(a => a.Lead)
                .AsNoTracking()
                .ToListAsync();

            return result;
        }

        public async Task DeleteAccountsByLeadIdAsync(int leadId)
        {
            var leadDb = await _context.Leads.SingleOrDefaultAsync(l => l.Id == leadId);
            if (leadDb == null)
            {
               // _logger.Warn($"{nameof(LeadEntity)} with id {leadId} not found.");
                throw new NotFoundException(leadId, nameof(LeadEntity));
            }
            else
            {
                await _context.Accounts.Where(
                    a => a.LeadId == leadId
                    && a.Status != AccountStatusEnum.Deactive && a.Status != AccountStatusEnum.Deleted)
                    .ForEachAsync(a => a.Status = AccountStatusEnum.Deleted);

                await _context.SaveChangesAsync();
            }
        }
    }
}