using AutoMapper;
using MakeYouPro.Bank.CRM.Bll.Models;
using MakeYouPro.Bource.CRM.Core.Enums;
using MakeYouPro.Bource.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Core.Clients.AuthService;
using MakeYouPro.Bourse.CRM.Core.Clients.AuthService.Models;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using MakeYouPro.Bourse.CRM.Core.Extensions;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Http.HttpResults;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using ILogger = NLog.ILogger;
using LogLevel = NLog.LogLevel;
using MakeYouPro.Bourse.CRM.Dal.Models;

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
            var leadEntity = await _leadRepository.GetLeadByIdAsync(leadId);

            if (leadEntity.IsDeleted)
            {
                _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(UpdateLeadUsingLeadAsync)}, Lead with id {leadId} is deleted");
                throw new ArgumentException($"Lead with id {leadId} is deleted");
            }
            else
            {
                var lead = _mapper.Map<Lead>(leadEntity);
                var accounts = lead.Accounts.Where(a => !a.IsDeleted).ToList();
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
                _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(DeleteLeadByIdAsync)}, You can delete lead only with status-Active");
                throw new ArgumentException("You can delete lead only with status-Active");
            }
        }

        public async Task<Lead> UpdateLeadUsingLeadAsync(Lead updateLead)
        {
            //ДОБАВИТЬ проверку по клеймам? о том, что чел изменяет инфу о себе
            // Во всех методах Добавить проверку? на то что чел залогинился

            var leadEntityDb = await _leadRepository.GetLeadByIdAsync(updateLead.Id);

            if (leadEntityDb.IsDeleted)
            {
                _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(UpdateLeadUsingLeadAsync)}, Lead with id {updateLead.Id} is deleted");
                throw new ArgumentException($"Lead with id {updateLead.Id} is deleted");
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
                    _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(UpdateLeadUsingLeadAsync)}, Lead has unsuitable role for update");
                    throw new ArgumentException($"Lead has unsuitable role for update");
                }
            }
        }

        public async Task<Lead> UpdateLeadUsingManagerAsync(Lead updateLead, int managerId)
        {
            var managerEntityDb = await _leadRepository.GetLeadByIdAsync(managerId);
            var leadEntityDb = await _leadRepository.GetLeadByIdAsync(updateLead.Id);

            if (leadEntityDb.IsDeleted || managerEntityDb.IsDeleted)
            {
                _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(UpdateLeadUsingLeadAsync)}, Lead or Manager is deleted");
                throw new ArgumentException($"Lead or Manager is deleted");
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
                    _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(UpdateLeadUsingLeadAsync)}, One of Leads has unsuitable role for update");
                    throw new ArgumentException($"One of Leads has unsuitable role for update");
                }
            }
        }

        public async Task<Lead> UpdateLeadRoleAsync(LeadRoleEnum leadRole, int leadId)
        {
            //Засунуть подписку на изменение сервиса
            // и проверки на то кто на какой статус меняет
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
                    var leadUpdateEntity = _leadRepository.UpdateLeadPhoneNumberAsync("0", leadDb.Id);
                    var result = await CreateLeadAsync(leadRequest);

                    return result;
                }
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
            var leadUpdateEntity = await _leadRepository.UpdateLeadAsync(leadEntityDb);
            await _leadRepository.ChangeIsDeletedLeadFromTrueToFalseAsync(leadDb.Id);
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