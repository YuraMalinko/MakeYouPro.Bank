using ReportingService.Dal.Models.CRM;
using ReportingService.Dal.Models.TransactionStore;

namespace ReportingService.Bll.IServices
{
    public interface IRecordingServices
    {
        Task CreateLeadInDatabaseAsync(LeadEntity lead);

        Task CreateAccountInDatabaseAsync(AccountEntity account);

        Task UpdateLeadInDatebaseAync(LeadEntity lead);

        Task UpdateAccountInDatebaseAync(AccountEntity account);

        Task CreateTransactionAsync(TransactionEntity transaction);

        Task UpdateTransactionAsync(TransactionEntity transaction);
    }
}