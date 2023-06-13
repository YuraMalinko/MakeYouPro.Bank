using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using MakeYouPro.Bourse.CRM.Dal.Models;
using Microsoft.EntityFrameworkCore;
using NLog;
using ILogger = NLog.ILogger;

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
                //leadDb.IsDeleted = false;
                await _context.SaveChangesAsync();

                return leadDb;
            }
        }


        //public async Task<List<LeadEntity>> GetLeadsByEmail(string email)
        //{
        //    string emailWithoutWhitespace = email.Replace(" ", String.Empty);

        //    return await _context.Leads
        //                .Where(l =>l.Email.Replace(" ", String.Empty) == emailWithoutWhitespace)
        //                .ToListAsync();
        //}

        //public async Task<List<LeadEntity>> GetLeadsByPhoneNumber(string phoneNumber)
        //{
        //    string phoneNumbertWithoutWhitespace = phoneNumber
        //                                                .Replace(" ", String.Empty)
        //                                                .Replace("-", String.Empty)
        //                                                .Replace("+", String.Empty);

        //    return await _context.Leads
        //                            .Where(l => l.PhoneNumber
        //                            .Replace(" ", String.Empty)
        //                            .Replace("-", String.Empty)
        //                            .Replace("+", "") == phoneNumbertWithoutWhitespace)
        //                            .ToListAsync();
        //}

        //public async Task<List<LeadEntity>> GetLeadsByPassport(string passport)
        //{
        //    string passportWithoutWhitespace = passport.Replace(" ","");

        //    return await _context.Leads
        //                .Where(l => l.PassportNumber.Replace(" ","") == passportWithoutWhitespace)
        //                .ToListAsync();
        //}

        public async Task<LeadEntity> GetLeadAsync(int id)
        {
            return (await _context.Leads
                .Where(l => l.Id == id)
                .Include(l => l.Accounts)
                .SingleOrDefaultAsync(l => l.Id == id))!;
        }
    }
}
