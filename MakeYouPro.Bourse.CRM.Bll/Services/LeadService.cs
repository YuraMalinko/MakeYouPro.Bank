using AutoMapper;
using MakeYouPro.Bourse.CRM.Auth.Bll.Models;
using MakeYouPro.Bourse.CRM.Auth.Dal.IRepository;
using MakeYouPro.Bourse.CRM.Auth.Dal.Models;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Clients.AuthService;
using MakeYouPro.Bourse.CRM.Core.Clients.AuthService.Models;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using MakeYouPro.Bourse.CRM.Dal.Models;
using Microsoft.Data.SqlClient;
using ILogger = NLog.ILogger;

namespace MakeYouPro.Bourse.CRM.Bll.Services
{
    public class LeadService : ILeadService
    {
        private readonly ILeadRepository _leadRepository;

        private readonly IUserRepository _userRepository;

        private readonly IAccountService _accountService;

        //  private readonly IAuthServiceClient _authServiceClient;

        private readonly IMapper _mapper;
        private readonly IMapper _huyaper;

        private readonly ILogger _logger;

        private SqlTransaction _transaction;

        public LeadService(ILeadRepository leadRepository,
            IAccountService accountService,
            IAuthServiceClient authServiceClient,
            IMapper mapper,
            IMapper huyaper,
            ILogger nLogger,
            IUserRepository userRepository
            )
        {
            _leadRepository = leadRepository;
            _accountService = accountService;
            // _authServiceClient = authServiceClient;
            _mapper = mapper;
            _logger = nLogger;
            _huyaper = huyaper;
            _userRepository = userRepository;
        }

        public async Task<Lead> CreateOrRecoverLeadAsync(Lead addLead)
        {
            _logger.Info($"{nameof(LeadService)} start {nameof(CreateOrRecoverLeadAsync)}");

            var leadsMatched = await GetLeadsWhosPropertiesAreMatchedAsync(addLead);

            switch (leadsMatched.Count)
            {
                case 0:
                    return await CreateLeadAsync(addLead);

                case 1:
                    return await RecoverOrThrowAsync(leadsMatched.First(), addLead);

                default:
                    throw new ArgumentException("2 or more properties (email/phoneNumber/passportNumber) belong to different Leads in database");
            };
        }

        public async Task<Lead> GetLeadByIdAsync(int leadId)
        {
            _logger.Info($"{nameof(LeadService)} start {nameof(GetLeadByIdAsync)}");

            var leadEntity = await _leadRepository.GetLeadByIdAsync(leadId);

            if (leadEntity.Status == LeadStatusEnum.Deleted || leadEntity.Status == LeadStatusEnum.Deactive)
            {
                throw new ArgumentException($"Lead with id {leadId} is deleted or is deactive");
            }
            else
            {
                var lead = _mapper.Map<Lead>(leadEntity);
                var accounts = lead.Accounts.Where(a => a.Status != AccountStatusEnum.Deleted).ToList();
                lead.Accounts = accounts;

                _logger.Info($"{nameof(LeadService)} end {nameof(GetLeadByIdAsync)}");

                return lead;
            }
        }

        public async Task DeleteLeadByIdAsync(int leadId)
        {
            _logger.Info($"{nameof(LeadService)} start {nameof(DeleteLeadByIdAsync)}");

            var leadEntity = await _leadRepository.GetLeadByIdAsync(leadId);

            if (leadEntity.Status == LeadStatusEnum.Active)
            {
                if (await UserDelete(leadEntity.Email))
                {
                    await _leadRepository.DeleteLeadByIdAsync(leadId);
                    await _accountService.DeleteAccountByLeadIdAsync(leadId);
                }
                else
                {
                    throw new WritingDataToServerException("The process failed because the user was not deleted");
                }

                _logger.Info($"{nameof(LeadService)} end {nameof(DeleteLeadByIdAsync)}");
            }
            else
            {
                throw new ArgumentException("You can delete lead only with status-Active");
            }
        }

        private async Task<bool> UserDelete(string email)
        {
            _logger.Info($"Start process deleted user by {email}");

            var dbUser = await _userRepository.GetUserByEmailAsync(email);
            var userInBase = _mapper.Map<User>(dbUser);

            if (userInBase != null && userInBase.Status != LeadStatusEnum.Deleted)
            {
                userInBase.Status = LeadStatusEnum.Deleted;
                var userToBase = _mapper.Map<UserEntity>(userInBase);
                var updateUser = _mapper.Map<User>(await _userRepository.UpdateUserAsync(userToBase));

                _logger.Info($"Сompletion process deleted user by {email}");

                return true;
            }
            _logger.Error($"Failed completion process deleted user by {email}");

            return false;
        }

        private async Task<User> UpdateUser(string email,Lead lead)
        {
            _logger.Info($"Start process update user {email}");


            var userInBase = _mapper.Map<User>(await _userRepository.GetUserByEmailAsync(email));

            if (userInBase != null)
            {
                userInBase.Status = lead.Status;
                userInBase.Role= lead.Role;
                userInBase.Email= lead.Email;

                var userToBase = _mapper.Map<UserEntity>(userInBase);
                var updateUser = _mapper.Map<User>(await _userRepository.UpdateUserAsync(userToBase));

                _logger.Info($"Сompletion process update user by {email}");

                return updateUser;
            }
            _logger.Error($"Failed completion process deleted user by {email}");

            return null;
        }



        public async Task<Lead> UpdateLeadUsingLeadAsync(Lead updateLead)
        {
            _logger.Info($"{nameof(LeadService)} start {nameof(UpdateLeadUsingLeadAsync)}");

            var leadEntityDb = await _leadRepository.GetLeadByIdAsync(updateLead.Id);

            if (leadEntityDb.Status == LeadStatusEnum.Deleted || leadEntityDb.Status == LeadStatusEnum.Deactive)
            {
                throw new ArgumentException($"Lead with id {updateLead.Id} is deleted or is deactive");
            }
            else
            {
                if (leadEntityDb.Role == LeadRoleEnum.StandartLead || leadEntityDb.Role == LeadRoleEnum.VipLead)
                {
                    

                    leadEntityDb.Name = updateLead.Name;
                    leadEntityDb.MiddleName = updateLead.MiddleName;
                    leadEntityDb.Surname = updateLead.Surname;
                    leadEntityDb.PhoneNumber = updateLead.PhoneNumber;
                    leadEntityDb.Comment = updateLead.Comment;

                    var updateLeadEntity = await _leadRepository.UpdateLeadAsync(leadEntityDb);
                    var result = _mapper.Map<Lead>(updateLeadEntity);

                    _logger.Info($"{nameof(LeadService)} end {nameof(UpdateLeadUsingLeadAsync)}");

                    return result;
                }
                else
                {
                    throw new ArgumentException($"Lead has unsuitable role for update");
                }
            }
        }

        public async Task<Lead> UpdateLeadUsingManagerAsync(Lead updateLead, int managerId)
        {
            _logger.Info($"{nameof(LeadService)} start {nameof(UpdateLeadUsingManagerAsync)}");

            var managerEntityDb = await _leadRepository.GetLeadByIdAsync(managerId);
            var leadEntityDb = await _leadRepository.GetLeadByIdAsync(updateLead.Id);

            if (leadEntityDb.Status == LeadStatusEnum.Deleted || managerEntityDb.Status == LeadStatusEnum.Deleted
                || leadEntityDb.Status == LeadStatusEnum.Deactive || managerEntityDb.Status == LeadStatusEnum.Deactive)
            {
                // _logger.Log(LogLevel.Warn, "Lead or Manager is deleted or deactive");
                throw new ArgumentException($"Lead or Manager is deleted or deactive");
            }
            else
            {
                if (managerEntityDb.Role == LeadRoleEnum.Manager
                    && (leadEntityDb.Role == LeadRoleEnum.VipLead || leadEntityDb.Role == LeadRoleEnum.StandartLead))
                {
                    //var userInBase = _mapper.Map<User>(await _userRepository.GetUserByEmailAsync(leadEntityDb.Email));
                    var emailUserInBase = leadEntityDb.Email;

                    leadEntityDb.Name = updateLead.Name;
                    leadEntityDb.MiddleName = updateLead.MiddleName;
                    leadEntityDb.Surname = updateLead.Surname;
                    leadEntityDb.PhoneNumber = updateLead.PhoneNumber;
                    leadEntityDb.Comment = updateLead.Comment;
                    leadEntityDb.Email = updateLead.Email;
                    leadEntityDb.Citizenship = updateLead.Citizenship;
                    leadEntityDb.Registration = updateLead.Registration;
                    leadEntityDb.PassportNumber = updateLead.PassportNumber;

                    var UpdateLeadToBase = _mapper.Map<Lead>(leadEntityDb);

                    var updateUser = await UpdateUser(emailUserInBase, UpdateLeadToBase);

                    if (updateUser == null)
                    {
                        throw new WritingDataToServerException($"update user bt email {emailUserInBase} has not been completed");
                    }

                    var updateLeadEntity = await _leadRepository.UpdateLeadAsync(leadEntityDb);
                    var result = _mapper.Map<Lead>(updateLeadEntity);

                    _logger.Info($"{nameof(LeadService)} end {nameof(UpdateLeadUsingManagerAsync)}");

                    return result;
                }
                else
                {
                    throw new ArgumentException($"One of Leads has unsuitable role for update");
                }
            }
        }

        public async Task<Lead> UpdateLeadRoleAsync(LeadRoleEnum leadRole, int leadId)
        {
            _logger.Info($"{nameof(LeadService)} start {nameof(UpdateLeadRoleAsync)}");

            var leadEntityDb = await _leadRepository.GetLeadByIdAsync(leadId);
            var leadToBase = _mapper.Map<Lead>(leadEntityDb);
            leadToBase.Role=leadRole;

            var updateUser = await UpdateUser(leadToBase.Email, leadToBase);

            if (updateUser == null)
            {
                throw new WritingDataToServerException($"update by email {leadToBase.Email} has not been completed");
            }

            var leadEntity = await _leadRepository.UpdateLeadRoleAsync(leadRole, leadId);
            var result = _mapper.Map<Lead>(leadEntity);

            _logger.Info($"{nameof(LeadService)} end {nameof(UpdateLeadRoleAsync)}");

            return result;
        }

        private async Task<Lead> CreateLeadAsync(Lead lead)
        {
            _logger.Info($"{nameof(LeadService)} start {nameof(CreateLeadAsync)}");

            lead.Role = LeadRoleEnum.StandartLead;
            lead.Status = LeadStatusEnum.Active;

            var userToBase = new User()
            {
                Email = lead.Email,
                Role = lead.Role,
                Status = lead.Status,
                Password = lead.Password!
            };

            var whriteUser = await AddUser(userToBase);

            if (whriteUser == null)
            {
                throw new WritingDataToServerException($"update by email {whriteUser.Email} has not been completed");
            }

            _logger.Info($"{whriteUser} was created");

            var leadEntity = _mapper.Map<LeadEntity>(lead);

            var addLeadEntity = _mapper.Map<Lead>(await _leadRepository.CreateLeadAsync(leadEntity));

            if (addLeadEntity == null)
            {
                _userRepository.UserDestruction(_mapper.Map<UserEntity>(whriteUser));
                throw new WritingDataToServerException($"{lead} is not recorded, the previously created {whriteUser} has been deleted");
            }

            if (addLeadEntity != null)
            {
                var defaultRubAccount = CreateDefaultRubAccount();
                defaultRubAccount.LeadId = addLeadEntity.Id;
                await _accountService.CreateOrRestoreAccountAsync(defaultRubAccount);

                var result = _mapper.Map<Lead>(addLeadEntity);

                _logger.Info($"{nameof(LeadService)} end {nameof(CreateLeadAsync)}");

                return result;
            }
            else
            {
                throw new ArgumentException("New lead did not created");
            }
        }

        private async Task<User> AddUser(User user)
        {
            _logger.Info($"Start the {user} generation process");

            if (!await _userRepository.CheckEmailAsync(user.Email))
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                user.Password = passwordHash;

                var userToBase = _mapper.Map<UserEntity>(user);
                var result = _mapper.Map<User>(await _userRepository.AddUserAsync(userToBase));

                _logger.Info($"Ends the {user} generation process");
                return result;
            }
            else
            {
                throw new RegistrationException($"User creation creation error due to mail or password");
            }
        }

        private async Task<Lead> RecoverOrThrowAsync(Lead leadDb, Lead leadRequest)
        {

            _logger.Info($"{nameof(LeadService)} start {nameof(RecoverOrThrowAsync)}");

            if (leadDb.Status == LeadStatusEnum.Active)
            {
                //  _logger.Log(LogLevel.Warn, "One of properties - email/phoneNumber/passportNumber belong to different Leads in database.");
                throw new AlreadyExistException(" one of properties - email/phoneNumber/passportNumber belong to different Leads in database");
            }
            else if (leadDb.Status == LeadStatusEnum.Deactive && leadRequest.Role != LeadRoleEnum.Manager)
            {
                // _logger.Log(LogLevel.Warn, "Only manager can restore deactive lead");
                throw new ArgumentException("Only manager can restore deactive lead");
            }
            else if (leadDb.Status == LeadStatusEnum.Deleted &&
                    (leadRequest.Role != LeadRoleEnum.StandartLead && leadRequest.Role != LeadRoleEnum.VipLead))
            {
                //  _logger.Log(LogLevel.Warn, "Only Lead can restore deleted lead");
                throw new ArgumentException("Only Lead can restore deleted lead");
            }
            else if (leadDb.Status == LeadStatusEnum.Deactive && leadRequest.Role == LeadRoleEnum.Manager
                || leadDb.Status == LeadStatusEnum.Deleted && (leadRequest.Role == LeadRoleEnum.StandartLead || leadRequest.Role == LeadRoleEnum.VipLead))
            {
                if (leadDb.PassportNumber == leadRequest.PassportNumber)
                {
                    return await UpdateLeadWhenCreateLeadHasSamePassportInDb(leadDb, leadRequest);
                }
                else if (leadDb.Email == leadRequest.Email && leadDb.PhoneNumber == leadRequest.PhoneNumber)
                {
                    //  _logger.Log(LogLevel.Warn, "Email and phoneNumber belong to other Lead in database.");
                    throw new AlreadyExistException("email and phoneNumber");
                }
                else if (leadDb.Email == leadRequest.Email)
                {
                    // _logger.Log(LogLevel.Warn, "Email belong to other Lead in database.");
                    throw new AlreadyExistException(nameof(LeadEntity.Email));
                }
                else if (leadDb.PhoneNumber == leadRequest.PhoneNumber)
                {
                    await _leadRepository.UpdateLeadPhoneNumberAsync("0", leadDb.Id);
                    var result = await CreateLeadAsync(leadRequest);

                    _logger.Info($"{nameof(LeadService)} end {nameof(RecoverOrThrowAsync)}");

                    return result;
                }
            }


            throw new ArgumentException();
        }

        private async Task<Lead> UpdateLeadWhenCreateLeadHasSamePassportInDb(Lead leadDb, Lead leadRequest)
        {
            _logger.Info($"{nameof(LeadService)} start {nameof(UpdateLeadWhenCreateLeadHasSamePassportInDb)}");

            var leadEntityDb = await _leadRepository.GetLeadByIdAsync(leadDb.Id);
            var oldEmailLead = leadEntityDb.Email;

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

            var leadTobase = _mapper.Map<Lead>(leadEntityDb);

            var updateUser = await UpdateUser(oldEmailLead, leadTobase);

            if (updateUser == null)
            {
                throw new WritingDataToServerException($"update by email {oldEmailLead} has not been completed");
            }

            var leadUpdateEntity = await _leadRepository.UpdateLeadAsync(leadEntityDb);
            await _leadRepository.RestoringDeletedStatusAsync(leadDb.Id);
            var result = _mapper.Map<Lead>(leadUpdateEntity);

            var defaultRubAccount = CreateDefaultRubAccount();
            defaultRubAccount.LeadId = result.Id;
            result.Accounts = new List<Account>
            {
                await _accountService.CreateOrRestoreAccountAsync(defaultRubAccount)
            };

            _logger.Info($"{nameof(LeadService)} end {nameof(UpdateLeadWhenCreateLeadHasSamePassportInDb)}");

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

        public async Task<Lead> GetLeadByEmail(string email)
        {
            return _mapper.Map<Lead>(await _leadRepository.GetLeadByEmail(email));
        }

        //private UserRegisterRequest CreateUserRegisterReguest(Lead addLead)
        //{
        //    UserRegisterRequest user = new UserRegisterRequest
        //    {
        //        Email = addLead.Email,
        //        Password = addLead.Password
        //    };

        //    return user;


        //}
    }
}