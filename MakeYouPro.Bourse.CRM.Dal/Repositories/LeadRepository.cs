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
            string emailWithoutwhitespace = email.Replace(" ", String.Empty);
            return await _context.Leads
                        .Where(l => l.Email.Replace(" ", String.Empty) == emailWithoutwhitespace)
                        .ToListAsync();
        }

        public async Task<List<LeadEntity>> GetLeadsByPhoneNumber(string phoneNumber)
        {
            string phoneNumbertWithoutwhitespace = phoneNumber.Replace(" ", String.Empty);
            return await _context.Leads
                        .Where(l => l.PhoneNumber.Replace(" ", String.Empty) == phoneNumber)
                        .ToListAsync();
        }

        public async Task<List<LeadEntity>> GetLeadsByPassport(string passport)
        {
            string passportWithoutwhitespace = passport.Replace(" ", String.Empty);
            var q = await _context.Leads
                        .Where(l => l.PassportNumber.Replace(" ", String.Empty) == passport)
                        .ToListAsync();

            return q;
        }
    }
}
