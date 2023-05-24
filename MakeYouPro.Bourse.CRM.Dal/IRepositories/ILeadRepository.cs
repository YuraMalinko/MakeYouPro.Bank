using MakeYouPro.Bource.CRM.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.CRM.Dal.IRepositories
{
    public interface ILeadRepository
    {
        Task<LeadEntity> CreateLeadAsync(LeadEntity lead);

        Task<List<LeadEntity>> GetLeadsByEmail(string email);

        Task<List<LeadEntity>> GetLeadsByPhoneNumber(string phoneNumber);

        Task<List<LeadEntity>> GetLeadsByPassport(string passport);
    }
}
