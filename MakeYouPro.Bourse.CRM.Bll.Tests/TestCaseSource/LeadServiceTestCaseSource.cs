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
                Status = Core.Enums.LeadStatusEnum.Active,
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

        public static IEnumerable DeleteLeadByIdAsync_WhenLeadStatusIsNotActive_ShouldBeArgumentException()
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
        
        }
    }
}
