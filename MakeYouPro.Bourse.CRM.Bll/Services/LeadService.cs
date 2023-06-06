using AutoMapper;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using MakeYouPro.Bourse.CRM.Core.Extensions;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Http.HttpResults;

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

        public async Task<Lead> CreateOrRecoverLeadAsync(Lead addLead)
        {
            //if (!await CheckEmailIsNotExistAsync(lead.Email))
            //{
            //    _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(CreateOrRecoverLeadAsync)}, this email is already exist.");
            //    throw new AlreadyExistException(nameof(LeadEntity.Email));
            //}

            //if (!await CheckPhoneNumberIsNotExistAsync(lead.PhoneNumber))
            //{
            //    _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(CreateOrRecoverLeadAsync)}, this phoneNumber is already exist.");
            //    throw new AlreadyExistException(nameof(LeadEntity.PhoneNumber));
            //}

            //if (!await CheckPassportIsNotExistAsync(lead.PassportNumber))
            //{
            //    _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(CreateOrRecoverLeadAsync)}, this passportNumber is already exist.");
            //    throw new AlreadyExistException(nameof(LeadEntity.PassportNumber));
            //}

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

        private async Task<Lead> CreateLeadAsync(Lead lead)
        {
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

        //private async Task<Lead> RecoverOrThrowAsync(Lead lead)
        //{
        //    var leadStatus = lead.Status;

        //    if (leadStatus == LeadStatusEnum.Deleted)
        //    {
        //        try
        //        {
        //            var updateLeadEntity = await _leadRepository.UpdateLeadStatus(LeadStatusEnum.Active, lead.Id);
        //            var result = _mapper.Map<Lead>(updateLeadEntity);

        //            return result;
        //        }
        //        catch (InvalidOperationException)
        //        {
        //            _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(RecoverOrThrowAsync)},Lead with id {lead.Id} is not found");
        //            throw new NotFoundException(lead.Id, nameof(LeadEntity));
        //        }
        //    }
        //    else
        //    {
        //        _logger.Log(LogLevel.Debug, $"{nameof(LeadService)} {nameof(LeadEntity)} {nameof(RecoverOrThrowAsync)}, email or phoneNumber or passportNumber is already exist.");
        //        throw new AlreadyExistException("email/phoneNumber/passportNumber");
        //    }
        //}

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

        //private async Task<bool> CheckEmailIsNotExistAsync(string email)
        //{
        //    var leads = await _leadRepository.GetLeadsByEmail(email);

        //    if (leads.Any())
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //private async Task<bool> CheckPhoneNumberIsNotExistAsync(string phoneNumber)
        //{
        //    var leads = await _leadRepository.GetLeadsByPhoneNumber(phoneNumber);
        //    var leadsActiveOrDeactive = leads.Where(l => l.Status == LeadStatusEnum.Active || l.Status == LeadStatusEnum.Deactive).ToList();

        //    if (leadsActiveOrDeactive.Any())
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //private async Task<bool> CheckPassportIsNotExistAsync(string passport)
        //{
        //    var leads = await _leadRepository.GetLeadsByPassport(passport);
        //    var leadsActiveOrDeactive = leads.Where(l => l.Status == LeadStatusEnum.Active || l.Status == LeadStatusEnum.Deactive).ToList();

        //    if (leadsActiveOrDeactive.Any())
        //    {
        //        return false;
        //    }

        //    return true;
        //}
    }
}