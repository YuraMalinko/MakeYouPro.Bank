using ReportingService.Dal.Models.CRM;

namespace ReportingService.Bll.IServices
{
    public interface IRecordingServices
    {
        Task CreateLeadInDatabaseAsync(LeadEntity lead);

        Task CreateAccountInDatabaseAsync(AccountEntity account);

        Task UpdateLeadInDatebaseAync(LeadEntity lead);

        Task UpdateAccountInDatebaseAync(AccountEntity account);
    }
}