using AutoMapper;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using NLog;
using ILogger = NLog.ILogger;

namespace MakeYouPro.Bourse.CRM.Bll.Services
{
    public class LeadService : ILeadService
    {
        private readonly ILeadRepository _leadRepository;

        private readonly IAccountService _accountService;

        private readonly IMapper _mapper;

        private readonly ILogger _logger;

        public LeadService(ILeadRepository leadRepository, IAccountService accountService, IMapper mapper, ILogger nLogger)
        {
            _leadRepository = leadRepository;
            _accountService = accountService;
            _mapper = mapper;
            _logger = nLogger;
        }

        public async Task<Lead> CreateLeadAsync(Lead lead)
        {
            if (!await CheckEmailIsNotExistAsync(lead.Email))
            {
                _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(CreateLeadAsync)}, this email is already exist.");
                throw new AlreadyExistException(nameof(LeadEntity.Email));
            }

            if (!await CheckPhoneNumberIsNotExistAsync(lead.PhoneNumber))
            {
                _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(CreateLeadAsync)}, this phoneNumber is already exist.");
                throw new AlreadyExistException(nameof(LeadEntity.PhoneNumber));
            }

            if (!await CheckPassportIsNotExistAsync(lead.PassportNumber))
            {
                _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(CreateLeadAsync)}, this passportNumber is already exist.");
                throw new AlreadyExistException(nameof(LeadEntity.PassportNumber));
            }

            var leadEntity = _mapper.Map<LeadEntity>(lead);
            //leadEntity.Role = LeadRoleEnum.StandardLead;
            //leadEntity.Status = LeadStatusEnum.Active;
            var addLeadEntity = await _leadRepository.CreateLeadAsync(leadEntity);

            if (addLeadEntity != null)
            {
                var defaultRubAccount = CreateDefaultRubAccount();
                defaultRubAccount.LeadId = addLeadEntity.Id;
                var addRubAccount = await _accountService.CreateAccountAsync(defaultRubAccount);

                var result = _mapper.Map<Lead>(addLeadEntity);
                result.Accounts.Add(addRubAccount);

                return result;
            }
            else
            {
                _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(CreateLeadAsync)}, New lead did not created");
                throw new ArgumentNullException("New lead did not created");
            }
        }

        private Account CreateDefaultRubAccount()
        {
            Account rubAccount = new Account
            {
                Currency = "RUB",
                Balance = 0,
                Status = (int)AccountStatusEnum.Active,
            };

            return rubAccount;
        }

        private async Task<bool> CheckEmailIsNotExistAsync(string email)
        {
            var leads = await _leadRepository.GetLeadsByEmail(email);

            if (leads.Any())
            {
                return false;
            }

            return true;
        }

        private async Task<bool> CheckPhoneNumberIsNotExistAsync(string phoneNumber)
        {
            var leads = await _leadRepository.GetLeadsByPhoneNumber(phoneNumber);
            var leadsActiveOrDeactive = leads.Where(l => l.Status == LeadStatusEnum.Active || l.Status == LeadStatusEnum.Deactive).ToList();

            if (leadsActiveOrDeactive.Any())
            {
                return false;
            }

            return true;
        }

        private async Task<bool> CheckPassportIsNotExistAsync(string passport)
        {
            var leads = await _leadRepository.GetLeadsByPassport(passport);
            var leadsActiveOrDeactive = leads.Where(l => l.Status == LeadStatusEnum.Active || l.Status == LeadStatusEnum.Deactive).ToList();

            if (leadsActiveOrDeactive.Any())
            {
                return false;
            }

            return true;
        }
    }
}