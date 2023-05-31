using ReportingService.Dal.Models.CRM;

namespace ReportingService.Bll.Services
{
    public interface IRecordingServices
    {
        Task CreateLeadInDatabaseAsync(LeadEntity lead);
    }
}