
using MakeYouPro.Bource.CRM.Dal;
using MakeYouPro.Bource.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
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
            return new AccountEntity();
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
