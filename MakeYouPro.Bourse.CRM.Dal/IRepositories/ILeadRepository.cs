using MakeYouPro.Bourse.CRM.Auth.Dal.Models;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Dal.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MakeYouPro.Bourse.CRM.Dal.IRepositories
{
    public interface ILeadRepository
    {
        Task<LeadEntity> CreateLeadAsync(LeadEntity lead);

        //Task<LeadEntity> CreateLeadAsync(LeadEntity lead,UserEntity user);

        Task<List<LeadEntity>> GetLeadsByPassportEmailPhoneAsync(LeadEntity lead);

        Task<LeadEntity> UpdateLeadStatusAsync(LeadStatusEnum leadStatus, int leadId);

        Task<LeadEntity> UpdateLeadAsync(LeadEntity leadUpdate);

        Task<LeadEntity> UpdateLeadPhoneNumberAsync(string phoneNumber, int leadId);

        Task<LeadEntity> RestoringDeletedStatusAsync(int leadId);

        Task<LeadEntity> GetLeadByIdAsync(int leadId);

        Task DeleteLeadByIdAsync(int leadId);

        Task<LeadEntity> UpdateLeadRoleAsync(LeadRoleEnum leadRole, int leadId);

        Task<LeadEntity> GetLeadByEmail(string email);

        //Task<Lead> LeadErasure(int leadId);

        //Task<Lead> FullUpdateLead(LeadEntity lead);
    }
}
