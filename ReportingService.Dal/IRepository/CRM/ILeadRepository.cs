using ReportingService.Dal.Models.CRM;

namespace ReportingService.Dal.IRepository.CRM
{
    public interface ILeadRepository
    {
        Task<LeadEntity> CreateLeadAsync(LeadEntity lead);

        Task UpdateLeadAsync(LeadEntity leadUpdate);
    }
}
