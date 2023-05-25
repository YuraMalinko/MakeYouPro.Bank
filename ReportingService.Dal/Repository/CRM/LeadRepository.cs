using Microsoft.EntityFrameworkCore;
using ReportingService.Dal.IRepository.CRM;
using ReportingService.Dal.Models.CRM;

namespace ReportingService.Dal.Repository.CRM
{
    public class LeadRepository : ILeadRepository
    {
        private static Context _context;

        public LeadRepository(Context context)
        {
            _context = context;
        }

        public async Task<LeadEntity> CreateLeadAsync(LeadEntity lead)
        {
            await _context.Leads.AddAsync(lead);
            await _context.SaveChangesAsync();

            return await _context.Leads
                .Include(l => l.Accounts)
                .SingleAsync(l => l.Id == lead.Id);
        }
    }
}
