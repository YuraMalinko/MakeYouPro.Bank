using MakeYouPro.Bourse.CRM.Bll.Models;

namespace MakeYouPro.Bourse.CRM.Bll.IServices
{
    public interface ILeadService
    {
        Task<Lead> CreateOrRecoverLeadAsync(Lead addLead);
    }
}
