using ReportingService.Bll.Models.CRM;

namespace ReportingService.Bll.IServices
{
    public interface ILeadService
    {
        Task<Lead> CreateLeadAsync(Lead lead);
    }
}
