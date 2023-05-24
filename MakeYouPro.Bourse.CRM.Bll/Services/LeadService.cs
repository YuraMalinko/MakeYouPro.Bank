using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using MakeYouPro.Bank.CRM.Bll.Models;
using MakeYouPro.Bource.CRM.Dal.Models;
using MakeYouPro.Bource.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Dal.Repositories;
using MakeYouPro.Bourse.CRM.Core.Enums;

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

        public async Task<Lead> CreateLead(Lead lead)
        {
            var leadEntity = _mapper.Map<LeadEntity>(lead);
            leadEntity.Role = (int)LeadRoleEnum.StandardLead;
            leadEntity.Status = (int)LeadStatusEnum.Active;
            var addLeadEntity = await _leadRepository.CreateLead(leadEntity);

            if (addLeadEntity != null)
            {
                var defaultRubAccount = CreateDefaultRubAccount();
                defaultRubAccount.LeadId = addLeadEntity.Id;
                var addRubAccount = await _accountService.CreateAccount(defaultRubAccount);

                var result = _mapper.Map<Lead>(addLeadEntity);
                result.Accounts.Add(addRubAccount);

                return result;
            }
            else
            {
                //ИСПРАВИТЬ
                return new Lead();
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
    }
}
