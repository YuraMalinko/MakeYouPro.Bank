
using MakeYouPro.Bourse.CRM.Dal;
using MakeYouPro.Bourse.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ILogger = NLog.ILogger;
using LogManager = NLog.LogManager;

namespace MakeYouPro.Bourse.CRM.Dal.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private CRMContext _context;
        private ILogger _logger;

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
                .SingleOrDefaultAsync(a => a.Id == account.Id))!;
        }
    }
}
