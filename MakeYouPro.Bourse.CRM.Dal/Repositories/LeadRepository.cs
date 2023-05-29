using MakeYouPro.Bourse.CRM.Dal;
using MakeYouPro.Bourse.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Runtime.CompilerServices;
using ILogger = NLog.ILogger;
using LogManager = NLog.LogManager;

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
            string emailWithoutWhitespace = email.Replace(" ", String.Empty);

            return await _context.Leads
                        .Where(l =>l.Email.Replace(" ", String.Empty) == emailWithoutWhitespace)
                        .ToListAsync();
        }

        public async Task<List<LeadEntity>> GetLeadsByPhoneNumber(string phoneNumber)
        {
            string phoneNumbertWithoutWhitespace = phoneNumber
                                                        .Replace(" ", String.Empty)
                                                        .Replace("-", String.Empty)
                                                        .Replace("+", String.Empty);

            return await _context.Leads
                                    .Where(l => l.PhoneNumber
                                    .Replace(" ", String.Empty)
                                    .Replace("-", String.Empty)
                                    .Replace("+", "") == phoneNumbertWithoutWhitespace)
                                    .ToListAsync();
        }

        public async Task<List<LeadEntity>> GetLeadsByPassport(string passport)
        {
            string passportWithoutWhitespace = passport.Replace(" ","");

            return await _context.Leads
                        .Where(l => l.PassportNumber.Replace(" ","") == passportWithoutWhitespace)
                        .ToListAsync();
        }

        public async Task<LeadEntity> GetLeadAsync(int id)
        {
            return (await _context.Leads
                .Where(l => l.Id == id)
                .Include(l => l.Accounts)
                .SingleOrDefaultAsync(l => l.Id == id))!;
        }
    }
}
