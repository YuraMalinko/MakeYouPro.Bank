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
    }
}
