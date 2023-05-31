using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReportingService.Dal.IRepository.CRM;
using ReportingService.Dal.Models.CRM;

namespace ReportingService.Dal.Repository.CRM
{
    public class AccountRepository : IAccountRepository
    {
        private Context _context;
        private ILogger _logger;

        public AccountRepository(Context context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<AccountEntity> CreateAccountAsync(AccountEntity account)
        {
            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();

            return (await _context.Accounts
                .SingleOrDefaultAsync(a => a.Id == account.Id))!;
        }
    }
}
