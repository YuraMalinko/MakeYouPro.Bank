using AutoMapper;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Clients.AuthService;
using MakeYouPro.Bourse.CRM.Core.Clients.AuthService.Models;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using MakeYouPro.Bourse.CRM.Dal.Models;
using ILogger = NLog.ILogger;
using LogLevel = NLog.LogLevel;

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
                    _logger.Log(LogLevel.Warn,  "2 or more properties (email/phoneNumber/passportNumber) belong to different Leads in database.");
                    throw new ArgumentException("2 or more properties (email/phoneNumber/passportNumber) belong to different Leads in database");
            };
        }

        public async Task<Lead> GetLeadById(int leadId)
        {
            var leadEntity = await _leadRepository.GetLeadByIdAsync(leadId);

            if (leadEntity.Status == LeadStatusEnum.Deleted || leadEntity.Status == LeadStatusEnum.Deactive)
            {
                _logger.Log(LogLevel.Warn, " Lead with id {leadId} is deleted or is deactive");
                throw new ArgumentException($"Lead with id {leadId} is deleted or is deactive");
            }
            else
            {
                var lead = _mapper.Map<Lead>(leadEntity);
                var accounts = lead.Accounts.Where(a => a.Status != AccountStatusEnum.Deleted).ToList();
                lead.Accounts = accounts;

                return lead;
            }
        }

        public async Task DeleteLeadByIdAsync(int leadId)
        {
            var leadEntity = await _leadRepository.GetLeadByIdAsync(leadId);

            if (leadEntity.Status == LeadStatusEnum.Active)
            {
                await _leadRepository.DeleteLeadByIdAsync(leadId);
                await _accountService.DeleteAccountByLeadIdAsync(leadId);
            }
            else
            {
                _logger.Log(LogLevel.Warn, " You can delete lead only with status-Active");
                throw new ArgumentException("You can delete lead only with status-Active");
            }
        }

        public async Task<Lead> UpdateLeadUsingLeadAsync(Lead updateLead)
        {
            //�������� �������� �� �������? � ���, ��� ��� �������� ���� � ����
            // �� ���� ������� �������� ��������? �� �� ��� ��� �����������

            var leadEntityDb = await _leadRepository.GetLeadByIdAsync(updateLead.Id);

            if (leadEntityDb.Status == LeadStatusEnum.Deleted || leadEntityDb.Status == LeadStatusEnum.Deactive)
            {
                _logger.Log(LogLevel.Warn, " Lead with id {updateLead.Id} is deleted or is deactive");
                throw new ArgumentException($"Lead with id {updateLead.Id} is deleted or is deactive");
            }
            else
            {
                if (leadEntityDb.Role == LeadRoleEnum.StandardLead || leadEntityDb.Role == LeadRoleEnum.VipLead)
                {
                    leadEntityDb.Name = updateLead.Name;
                    leadEntityDb.MiddleName = updateLead.MiddleName;
                    leadEntityDb.Surname = updateLead.Surname;
                    leadEntityDb.PhoneNumber = updateLead.PhoneNumber;
                    leadEntityDb.Comment = updateLead.Comment;

                    var updateLeadEntity = await _leadRepository.UpdateLeadAsync(leadEntityDb);
                    var result = _mapper.Map<Lead>(updateLeadEntity);

                    return result;
                }
                else
                {
                    _logger.Log(LogLevel.Warn, "Lead has unsuitable role for update");
                    throw new ArgumentException($"Lead has unsuitable role for update");
                }
            }
        }

        public async Task<Lead> UpdateLeadUsingManagerAsync(Lead updateLead, int managerId)
        {
            var managerEntityDb = await _leadRepository.GetLeadByIdAsync(managerId);
            var leadEntityDb = await _leadRepository.GetLeadByIdAsync(updateLead.Id);

            if (leadEntityDb.Status == LeadStatusEnum.Deleted || managerEntityDb.Status == LeadStatusEnum.Deleted
                || leadEntityDb.Status == LeadStatusEnum.Deactive || managerEntityDb.Status == LeadStatusEnum.Deactive)
            {
                _logger.Log(LogLevel.Warn, "Lead or Manager is deleted or deactive");
                throw new ArgumentException($"Lead or Manager is deleted or deactive");
            }
            else
            {
                if (managerEntityDb.Role == LeadRoleEnum.Manager
                    && (leadEntityDb.Role == LeadRoleEnum.VipLead || leadEntityDb.Role == LeadRoleEnum.StandardLead))
                {
                    leadEntityDb.Name = updateLead.Name;
                    leadEntityDb.MiddleName = updateLead.MiddleName;
                    leadEntityDb.Surname = updateLead.Surname;
                    leadEntityDb.PhoneNumber = updateLead.PhoneNumber;
                    leadEntityDb.Comment = updateLead.Comment;
                    leadEntityDb.Email = updateLead.Email;
                    leadEntityDb.Citizenship = updateLead.Citizenship;
                    leadEntityDb.Registration = updateLead.Registration;
                    leadEntityDb.PassportNumber = updateLead.PassportNumber;
                    var updateLeadEntity = await _leadRepository.UpdateLeadAsync(leadEntityDb);
                    var result = _mapper.Map<Lead>(updateLeadEntity);

                    return result;
                }
                else
                {
                    _logger.Log(LogLevel.Warn, " One of Leads has unsuitable role for update");
                    throw new ArgumentException($"One of Leads has unsuitable role for update");
                }
            }
        }

        public async Task<Lead> UpdateLeadRoleAsync(LeadRoleEnum leadRole, int leadId)
        {
            //�������� �������� �� ��������� �������
            // � �������� �� �� ��� �� ����� ������ ������
            var leadEntity = await _leadRepository.UpdateLeadRoleAsync(leadRole, leadId);
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
                var addRubAccount = await _accountService.CreateOrRestoreAccountAsync(defaultRubAccount);

                var result = _mapper.Map<Lead>(addLeadEntity);

                return result;
            }
            else
            {
                _logger.Log(LogLevel.Warn, " New lead did not created");
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
            // � ���������� � ������� �y� ���� ����� ���� ��������� � ��� �� ������ ��� ���� //
            if (leadDb.Status == LeadStatusEnum.Active)
            {
                _logger.Log(LogLevel.Warn, "One of properties - email/phoneNumber/passportNumber belong to different Leads in database.");
                throw new AlreadyExistException(" one of properties - email/phoneNumber/passportNumber belong to different Leads in database");
            }
            else if (leadDb.Status == LeadStatusEnum.Deactive && leadRequest.Role == LeadRoleEnum.Manager
                || leadDb.Status == LeadStatusEnum.Deleted && (leadRequest.Role == LeadRoleEnum.StandardLead || leadRequest.Role == LeadRoleEnum.VipLead))
            {
                if (leadDb.PassportNumber == leadRequest.PassportNumber)
                {
                    return await UpdateLeadWhenCreateLeadHasSamePassportInDb(leadDb, leadRequest);
                }
                else if (leadDb.Email == leadRequest.Email && leadDb.PhoneNumber == leadRequest.PhoneNumber)
                {
                    _logger.Log(LogLevel.Warn, "Email and phoneNumber belong to other Lead in database.");
                    throw new AlreadyExistException("email and phoneNumber");
                }
                else if (leadDb.Email == leadRequest.Email)
                {
                    _logger.Log(LogLevel.Warn, "Email belong to other Lead in database.");
                    throw new AlreadyExistException(nameof(LeadEntity.Email));
                }
                else if (leadDb.PhoneNumber == leadRequest.PhoneNumber)
                {
                    await _leadRepository.UpdateLeadPhoneNumberAsync("0", leadDb.Id);
                    var result = await CreateLeadAsync(leadRequest);

                    return result;
                }
            }
            else if (leadDb.Status == LeadStatusEnum.Deactive && leadRequest.Role != LeadRoleEnum.Manager)
            {
                _logger.Log(LogLevel.Warn, "Only manager can restore deactive lead");
                throw new ArgumentException("Only manager can restore deactive lead");
            }
            else if (leadDb.Status == LeadStatusEnum.Deleted && 
                    (leadRequest.Role != LeadRoleEnum.StandardLead || leadRequest.Role != LeadRoleEnum.VipLead))
            {
                _logger.Log(LogLevel.Warn, "Only Lead can restore deleted lead");
                throw new ArgumentException("Only Lead can restore deleted lead");
            }

            throw new ArgumentException();
        }

        private async Task<Lead> UpdateLeadWhenCreateLeadHasSamePassportInDb(Lead leadDb, Lead leadRequest)
        {
            var leadEntityDb = await _leadRepository.GetLeadByIdAsync(leadDb.Id);

            leadEntityDb.Name = leadRequest.Name;
            leadEntityDb.MiddleName = leadRequest.MiddleName;
            leadEntityDb.Surname = leadRequest.Surname;
            leadEntityDb.PhoneNumber = leadRequest.PhoneNumber;
            leadEntityDb.Comment = leadRequest.Comment;
            leadEntityDb.Email = leadRequest.Email;
            leadEntityDb.Citizenship = leadRequest.Citizenship;
            leadEntityDb.Registration = leadRequest.Registration;
            leadEntityDb.PassportNumber = leadRequest.PassportNumber;
            leadEntityDb.Status = LeadStatusEnum.Active;

            var leadUpdateEntity = await _leadRepository.UpdateLeadAsync(leadEntityDb);
            await _leadRepository.RestoringDeletedStatusAsync(leadDb.Id);
            var result = _mapper.Map<Lead>(leadUpdateEntity);

            var defaultRubAccount = CreateDefaultRubAccount();
            defaultRubAccount.LeadId = result.Id;
            await _accountService.CreateOrRestoreAccountAsync(defaultRubAccount);

            return result;
        }

        private Account CreateDefaultRubAccount()
        {
            Account rubAccount = new Account
            {
                Currency = "RUB"
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