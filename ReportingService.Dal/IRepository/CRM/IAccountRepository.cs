using ReportingService.Dal.Models.CRM;

namespace ReportingService.Dal.IRepository.CRM
{
    public interface IAccountRepository
    {
        Task<AccountEntity> CreateAccountAsync(AccountEntity account);

        Task UpdateAccountAsync(AccountEntity accountUpdate);
    }
}
