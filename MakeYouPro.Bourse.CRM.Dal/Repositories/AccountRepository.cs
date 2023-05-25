
using MakeYouPro.Bource.CRM.Dal;
using MakeYouPro.Bource.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<AccountEntity> UpdateAccountAsync(AccountEntity account)
        {
            var oldAccount = await _context.Accounts
                .SingleOrDefaultAsync(a => a.Id == account.Id);

            if (oldAccount != null || oldAccount!.Status==AccountStatusEnum.Active)
            {
                oldAccount.Comment = account.Comment;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentNullException("");
            }

            return oldAccount;
        }
    }
}
