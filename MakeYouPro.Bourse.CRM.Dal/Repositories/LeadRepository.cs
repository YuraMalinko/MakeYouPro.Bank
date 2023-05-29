using MakeYouPro.Bource.CRM.Core.Enums;
using MakeYouPro.Bource.CRM.Dal;
using MakeYouPro.Bource.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using Microsoft.EntityFrameworkCore;
using NLog;

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

        public async Task<List<LeadEntity>> GetLeadsByPassportEmailPhoneAsync(LeadEntity lead)
        {
            return await _context.Leads
                            .Where(l =>
                              (l.PassportNumber == lead.PassportNumber && l.Citizenship == lead.Citizenship)
                            || l.Email == lead.Email
                            || l.PhoneNumber == lead.PhoneNumber)
                            .ToListAsync();
        }

        public async Task<LeadEntity> UpdateLeadStatus(LeadStatusEnum leadStatus, int leadId)
        {
            var leadDb = await _context.Leads.SingleOrDefaultAsync(l => l.Id == leadId);

            if (leadDb == null)
            {
                _logger.Log(LogLevel.Debug, $"{nameof(LeadEntity)} with id {leadId} not found.");
                throw new NotFoundException(leadId, nameof(LeadEntity));
            }
            else
            {
                leadDb.Status = leadStatus;
                await _context.SaveChangesAsync();

                return leadDb;
            }
        }

        public async Task<LeadEntity> UpdateLead(LeadEntity leadUpdate)
        {
            var leadDb = await _context.Leads.SingleOrDefaultAsync(l => l.Id == leadUpdate.Id);

            if (leadDb == null)
            {
                _logger.Log(LogLevel.Debug, $"{nameof(LeadEntity)} with id {leadUpdate.Id} not found.");
                throw new NotFoundException(leadUpdate.Id, nameof(LeadEntity));
            }
            else
            {
                leadDb.Surname = leadUpdate.Surname;
                leadDb.Name = leadUpdate.Name;
                leadDb.MiddleName = leadUpdate.MiddleName;
                leadDb.PhoneNumber = leadUpdate.PhoneNumber;
                leadDb.Email = leadUpdate.Email;
                leadDb.Citizenship = leadUpdate.Citizenship;
                leadDb.PassportNumber = leadUpdate.PassportNumber;
                leadDb.Registration = leadUpdate.Registration;
                leadDb.Comment = leadUpdate.Comment;
                await _context.SaveChangesAsync();

                return leadDb;
            }
        }

        public async Task<LeadEntity> UpdateLeadPhoneNumber(string phoneNumber, int leadId)
        {
            var leadDb = await _context.Leads.SingleOrDefaultAsync(l => l.Id == leadId);

            if (leadDb == null)
            {
                _logger.Log(LogLevel.Debug, $"{nameof(LeadEntity)} with id {leadId} not found.");
                throw new NotFoundException(leadId, nameof(LeadEntity));
            }
            else
            {
                leadDb.PhoneNumber = phoneNumber;
                await _context.SaveChangesAsync();

                return leadDb;
            }
        }

        public async Task<LeadEntity> ChangeIsDeletedLeadFromTrueToFalse(int leadId)
        {
            var leadDb = await _context.Leads.SingleOrDefaultAsync(l => l.Id == leadId);

            if (leadDb == null)
            {
                _logger.Log(LogLevel.Debug, $"{nameof(LeadEntity)} with id {leadId} not found.");
                throw new NotFoundException(leadId, nameof(LeadEntity));
            }
            else
            {
                leadDb.IsDeleted = false;
                await _context.SaveChangesAsync();

                return leadDb;
            }
        }
    }
}
