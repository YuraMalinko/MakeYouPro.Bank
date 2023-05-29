using MakeYouPro.Bource.CRM.Core.Enums;
using MakeYouPro.Bource.CRM.Dal.Models;

namespace MakeYouPro.Bourse.CRM.Dal.IRepositories
{
    public interface ILeadRepository
    {
        Task<LeadEntity> CreateLeadAsync(LeadEntity lead);

        Task<List<LeadEntity>> GetLeadsByPassportEmailPhoneAsync(LeadEntity lead);

        Task<LeadEntity> UpdateLeadStatus(LeadStatusEnum leadStatus, int leadId);

        Task<LeadEntity> UpdateLead(LeadEntity leadUpdate);

        Task<LeadEntity> UpdateLeadPhoneNumber(string phoneNumber, int leadId);

        Task<LeadEntity> ChangeIsDeletedLeadFromTrueToFalse(int leadId);

        Task<LeadEntity> GetLeadById(int leadId);
    }
}
