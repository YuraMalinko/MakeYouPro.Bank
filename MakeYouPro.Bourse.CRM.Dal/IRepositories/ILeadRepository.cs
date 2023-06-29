using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Dal.Models;

namespace MakeYouPro.Bourse.CRM.Dal.IRepositories
{
    public interface ILeadRepository
    {
        Task<LeadEntity> CreateLeadAsync(LeadEntity lead);

        Task<List<LeadEntity>> GetLeadsByPassportEmailPhoneAsync(LeadEntity lead);

        Task<LeadEntity> UpdateLeadStatusAsync(LeadStatusEnum leadStatus, int leadId);

        Task<LeadEntity> UpdateLeadAsync(LeadEntity leadUpdate);

        Task<LeadEntity> UpdateLeadPhoneNumberAsync(string phoneNumber, int leadId);

        Task<LeadEntity> RestoringDeletedStatusAsync(int leadId);

        Task<LeadEntity> GetLeadByIdAsync(int leadId);

        Task DeleteLeadByIdAsync(int leadId);

        Task<LeadEntity> GetLeadByEmail(string email);
        
        Task<LeadEntity> UpdateLeadRoleAsync(int leadRole, int leadId);
    }
}
