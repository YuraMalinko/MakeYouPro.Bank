using MakeYouPro.Bank.CRM.Bll.Models;

namespace MakeYouPro.Bourse.CRM.Bll.IServices
{
    public interface ILeadService
    {
        Task<Lead> CreateOrRecoverLeadAsync(Lead addLead);

        Task<Lead> GetLeadById(int leadId);

        Task DeleteLeadByIdAsync(int leadId);

        Task<Lead> UpdateLeadUsingLeadAsync(Lead updateLead);

        Task<Lead> UpdateLeadUsingManagerAsync(Lead updateLead, int managerId);
    }
}
