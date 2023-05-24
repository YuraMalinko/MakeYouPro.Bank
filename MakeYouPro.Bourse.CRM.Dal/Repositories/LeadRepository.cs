using MakeYouPro.Bource.CRM.Dal;
using MakeYouPro.Bource.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Runtime.CompilerServices;

namespace MakeYouPro.Bourse.CRM.Dal.Repositories
{
    public class LeadRepository : ILeadRepository
    {
        private static CRMContext _context;

        private readonly ILogger _logger;

        public LeadRepository(CRMContext context, ILogger nLogger)
        {
            _context = context;
            _logger = nLogger;
        }

        public async Task<LeadEntity> CreateLeadAsync(LeadEntity lead)
        {
            await _context.Leads.AddAsync(lead);
            //lead.DateCreate = DateTime.UtcNow;
            await _context.SaveChangesAsync();


            return await _context.Leads
                .Include(l => l.Accounts)
                .SingleAsync(l => l.Id == lead.Id);
        }

        public async Task<List<LeadEntity>> GetLeadsByEmail(string email)
        {
            return await _context.Leads
                        .Where(l => l.Email == email)
                        .ToListAsync();
        }
    }
}
