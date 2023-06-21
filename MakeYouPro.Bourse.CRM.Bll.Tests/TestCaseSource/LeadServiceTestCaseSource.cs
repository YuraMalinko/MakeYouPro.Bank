using MakeYouPro.Bourse.CRM.Bll.Models;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Dal.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.CRM.Bll.Tests.TestCaseSource
{
    public class LeadServiceTestCaseSource
    {
        public static IEnumerable DeleteLeadByIdAsyncTestCaseSource()
        {
            int leadId = 10;
            LeadEntity leadEntity = new LeadEntity
            {
                Id = 10,
                Status = LeadStatusEnum.Active,
                Accounts = new List<AccountEntity>
                {
                new AccountEntity
                    {
                        Id = 1,
                        LeadId = 10
                    }
                }
            };

            yield return new object[] { leadId, leadEntity };
        }

        public static IEnumerable DeleteLeadByIdAsyncTestCaseSource_WhenLeadStatusIsNotActive_ShouldBeArgumentException()
        {
            //1. StatusLead = Deactive
            int leadId = 11;
            LeadEntity leadEntity = new LeadEntity
            {
                Id = 11,
                Status = Core.Enums.LeadStatusEnum.Deactive,
                Accounts = new List<AccountEntity>
                {
                new AccountEntity
                    {
                        Id = 11,
                        LeadId = 11
                    }
                }
            };

            yield return new object[] { leadId, leadEntity };

            //2. StatusLead = Deleted
            leadId = 113;
            leadEntity = new LeadEntity
            {
                Id = 113,
                Status = Core.Enums.LeadStatusEnum.Deleted,
                Accounts = new List<AccountEntity>
                {
                new AccountEntity
                    {
                        Id = 113,
                        LeadId = 113
                    }
                }
            };

            yield return new object[] { leadId, leadEntity };
        }

        public static IEnumerable GetLeadByIdAsyncTestCaseSource()
        {
            //1. Проверка с активным лидом и активным аккаунтом

            int leadId = 104;
            LeadEntity leadEntity = new LeadEntity
            {
                Id = 104,
                Status = LeadStatusEnum.Active,
                Accounts = new List<AccountEntity>
                {
                new AccountEntity
                    {
                        Id = 14,
                        LeadId = 104,
                        Status = AccountStatusEnum.Active
                    }
                }
            };
            Lead expected = new Lead
            {
                Id = 104,
                Status = LeadStatusEnum.Active,
                Accounts = new List<Account>
                {
                new Account
                    {
                        Id = 14,
                        LeadId = 104,
                        Status = AccountStatusEnum.Active
                    }
                }
            };

            yield return new object[] { leadId, leadEntity, expected };

            //2. Проверка с активным лидом и активным и удаленным аккаунтом - выведется только активный акаунт

            leadId = 1045;
            leadEntity = new LeadEntity
            {
                Id = 1045,
                Status = LeadStatusEnum.Active,
                Accounts = new List<AccountEntity>
                {
                new AccountEntity
                    {
                        Id = 145,
                        LeadId = 1045,
                        Status = AccountStatusEnum.Active
                    },
                new AccountEntity
                    {
                        Id = 12,
                        LeadId = 1045,
                        Status = AccountStatusEnum.Deleted
                    }
                }
            };
            expected = new Lead
            {
                Id = 1045,
                Status = LeadStatusEnum.Active,
                Accounts = new List<Account>
                {
                new Account
                    {
                        Id = 145,
                        LeadId = 1045,
                        Status = AccountStatusEnum.Active
                    }
                }
            };

            yield return new object[] { leadId, leadEntity, expected };
        }

        public static IEnumerable GetLeadByIdAsyncTestCaseSource_WhenTeadStatusIsNotActive_ShouldBeArgumentException()
        {
            //1. LeadStatus = Deleted

            int leadId = 1049;
            LeadEntity leadEntity = new LeadEntity
            {
                Id = 1049,
                Status = LeadStatusEnum.Deleted,
                Accounts = new List<AccountEntity>
                {
                new AccountEntity
                    {
                        Id = 149,
                        LeadId = 1049,
                        Status = AccountStatusEnum.Deleted
                    }
                }
            };

            yield return new object[] { leadId, leadEntity };

            //2. LeadStatus = Deactive

            leadId = 10498;
            leadEntity = new LeadEntity
            {
                Id = 10498,
                Status = LeadStatusEnum.Deactive,
                Accounts = new List<AccountEntity>
                {
                new AccountEntity
                    {
                        Id = 1498,
                        LeadId = 10498,
                        Status = AccountStatusEnum.Deleted
                    }
                }
            };

            yield return new object[] { leadId, leadEntity };
        }
    }
}
