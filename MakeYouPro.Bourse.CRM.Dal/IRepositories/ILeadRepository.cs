using MakeYouPro.Bourse.CRM.Dal.Models;

namespace MakeYouPro.Bourse.CRM.Dal.IRepositories
{
    public interface ILeadRepository
    {
        Task<LeadEntity> CreateLeadAsync(LeadEntity lead);

        Task<List<LeadEntity>> GetLeadsByEmail(string email);

        Task<List<LeadEntity>> GetLeadsByPhoneNumber(string phoneNumber);

        Task<List<LeadEntity>> GetLeadsByPassport(string passport);

        Task<LeadEntity> GetLeadAsync(int id);
    }
}
