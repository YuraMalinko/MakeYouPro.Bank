using AutoMapper;
using MakeYouPro.Bank.CRM.Bll.Models;
using MakeYouPro.Bource.CRM.Core.Enums;
using MakeYouPro.Bource.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Core.Clients.AuthService;
using MakeYouPro.Bourse.CRM.Core.Clients.AuthService.Models;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using NLog;

namespace MakeYouPro.Bourse.CRM.Bll.Services
{
    public class LeadService : ILeadService
    {
        private readonly ILeadRepository _leadRepository;

        private readonly IAccountService _accountService;

        private readonly IAuthServiceClient _authServiceClient;

        private readonly IMapper _mapper;

        private readonly ILogger _logger;

        public LeadService(ILeadRepository leadRepository, IAccountService accountService, IAuthServiceClient authServiceClient, IMapper mapper, ILogger nLogger)
        {
            _leadRepository = leadRepository;
            _accountService = accountService;
            _authServiceClient = authServiceClient;
            _mapper = mapper;
            _logger = nLogger;
        }

        public async Task<Lead> CreateOrRecoverLeadAsync(Lead addLead)
        {
            var leadsMatched = await GetLeadsWhosPropertiesAreMatchedAsync(addLead);

            switch (leadsMatched.Count)
            {
                case 0:
                    return await CreateLeadAsync(addLead);

                case 1:
                    return await RecoverOrThrowAsync(leadsMatched.First(), addLead);

                default:
                    _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(CreateOrRecoverLeadAsync)}, 2 or more properties (email/phoneNumber/passportNumber) belong to different Leads in database.");
                    throw new ArgumentException("2 or more properties (email/phoneNumber/passportNumber) belong to different Leads in database");
            };
        }

        public async Task<Lead> GetLeadById(int leadId)
        {
            var leadEntity = await _leadRepository.GetLeadById(leadId);
            var result = _mapper.Map<Lead>(leadEntity);

            return result;
        }

        private async Task<Lead> CreateLeadAsync(Lead lead)
        {
            //try
            //{
            //    await _authServiceClient.RegisterAsync(CreateUserRegisterReguest(lead));
            //}
            //catch ()
            //{ 

            //}

            var leadEntity = _mapper.Map<LeadEntity>(lead);
            leadEntity.Role = LeadRoleEnum.StandardLead;
            leadEntity.Status = LeadStatusEnum.Active;
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
                _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(CreateOrRecoverLeadAsync)}, New lead did not created");
                throw new ArgumentException("New lead did not created");
            }
        }

        private UserRegisterRequest CreateUserRegisterReguest(Lead addLead)
        {
            UserRegisterRequest user = new UserRegisterRequest
            {
                Email = addLead.Email,
                Password = addLead.Password
            };

            return user;
        }

        private async Task<Lead> RecoverOrThrowAsync(Lead leadDb, Lead leadRequest)
        {
            if (!leadDb.IsDeleted)
            {
                _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(RecoverOrThrowAsync)}, one of properties - email/phoneNumber/passportNumber belong to different Leads in database.");
                throw new AlreadyExistException(" one of properties - email/phoneNumber/passportNumber belong to different Leads in database");
            }
            else
            {
                if (leadDb.PassportNumber == leadRequest.PassportNumber)
                {
                    return await UpdateLeadWhenCreateLeadHasSamePassportInDb(leadDb, leadRequest);
                }
                else if (leadDb.Email == leadRequest.Email && leadDb.PhoneNumber == leadRequest.PhoneNumber)
                {
                    _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(RecoverOrThrowAsync)}, email and phoneNumber belong to other Lead in database.");
                    throw new AlreadyExistException("email and phoneNumber");
                }
                else if (leadDb.Email == leadRequest.Email)
                {
                    _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(RecoverOrThrowAsync)}, email belong to other Lead in database.");
                    throw new AlreadyExistException(nameof(LeadEntity.Email));
                }
                else if (leadDb.PhoneNumber == leadRequest.PhoneNumber)
                {
                    var leadUpdateEntity = _leadRepository.UpdateLeadPhoneNumber("0", leadDb.Id);
                    var result = await CreateLeadAsync(leadRequest);

                    return result;
                }
            }
            throw new ArgumentException();
        }

        private async Task<Lead> UpdateLeadWhenCreateLeadHasSamePassportInDb(Lead leadDb, Lead leadRequest)
        {
            var leadRequestEntity = _mapper.Map<LeadEntity>(leadRequest);
            leadRequestEntity.Id = leadDb.Id;
            var leadUpdateEntity = await _leadRepository.UpdateLead(leadRequestEntity);
            await _leadRepository.ChangeIsDeletedLeadFromTrueToFalse(leadDb.Id);
            var result = _mapper.Map<Lead>(leadUpdateEntity);
            var defaultRubAccount = CreateDefaultRubAccount();
            defaultRubAccount.LeadId = result.Id;
            await _accountService.CreateAccountAsync(defaultRubAccount);

            return result;
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

        private async Task<List<Lead>> GetLeadsWhosPropertiesAreMatchedAsync(Lead lead)
        {
            var leadEntity = _mapper.Map<LeadEntity>(lead);
            var leads = await _leadRepository.GetLeadsByPassportEmailPhoneAsync(leadEntity);
            var result = _mapper.Map<List<Lead>>(leads);

            return result;
        }
    }
}
