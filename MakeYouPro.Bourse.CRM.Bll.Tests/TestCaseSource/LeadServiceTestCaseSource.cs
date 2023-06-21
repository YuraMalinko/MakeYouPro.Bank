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

        public static IEnumerable CreateOrRecoverLeadAsyncTestCaseSource_WhenCreateLead()
        {
            LeadEntity leadEntity = new LeadEntity
            {
                PassportNumber = "1111 000000",
                Email = "1@mail.ru",
                PhoneNumber = "8921002232",
                Name = "1",
                Citizenship = "RUS"
            };
            List<LeadEntity> leads = new List<LeadEntity>();
            LeadEntity addLeadEntity = new LeadEntity
            {
                Id = 1,
                PassportNumber = "1111 000000",
                Email = "1@mail.ru",
                PhoneNumber = "8921002232",
                Name = "1",
                Citizenship = "RUS"
            };
            Lead addLead = new Lead
            {
                PassportNumber = "1111 000000",
                Email = "1@mail.ru",
                PhoneNumber = "8921002232",
                Name = "1",
                Citizenship = "RUS"
            };
            Lead expected = new Lead
            {
                Id = 1,
                PassportNumber = "1111 000000",
                Email = "1@mail.ru",
                PhoneNumber = "8921002232",
                Name = "1",
                Citizenship = "RUS"
            };

            yield return new object[] { leadEntity, leads, addLeadEntity, addLead, expected };
        }

        public static IEnumerable CreateOrRecoverLeadAsyncTestCaseSource_WhenRecoverLeadWithSamePasport()
        {
            //1. Случай, когда ВипЛид восстанавливается (по совпадению по паспорту)
            LeadEntity leadEntity = new LeadEntity
            {
                PassportNumber = "1111 0000002",
                Email = "12@mail.ru",
                PhoneNumber = "89210022322",
                Name = "12",
                Citizenship = "RUS"
            };
            List<LeadEntity> leads = new List<LeadEntity>
            {
            new LeadEntity
                {
                Id = 30,
                PassportNumber = "1111 0000002",
                Email = "1230@mail.ru",
                PhoneNumber = "89210022330",
                Name = "1230",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Deleted,
                Role = LeadRoleEnum.VipLead
                }
            };
            Lead leadMatchedDb = new Lead
            {
                Id = 30,
                PassportNumber = "1111 0000002",
                Email = "1230@mail.ru",
                PhoneNumber = "89210022330",
                Name = "1230",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Deleted,
                Role = LeadRoleEnum.VipLead
            };
            LeadEntity leadEntityDb = new LeadEntity
            {
                Id = 30,
                PassportNumber = "1111 0000002",
                Email = "1230@mail.ru",
                PhoneNumber = "89210022330",
                Name = "1230",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Deleted,
                Role = LeadRoleEnum.VipLead
            };
            LeadEntity leadUpdateEntity = new LeadEntity
            {
                Id = 30,
                PassportNumber = "1111 0000002",
                Email = "12@mail.ru",
                PhoneNumber = "89210022322",
                Name = "12",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Active,
                Role = LeadRoleEnum.VipLead
            };
            Lead addLead = new Lead
            {
                PassportNumber = "1111 0000002",
                Email = "12@mail.ru",
                PhoneNumber = "89210022322",
                Name = "12",
                Citizenship = "RUS"
            };
            Lead expected = new Lead
            {
                Id = 30,
                PassportNumber = "1111 0000002",
                Email = "12@mail.ru",
                PhoneNumber = "89210022322",
                Name = "12",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Active,
                Role = LeadRoleEnum.VipLead
            };

            yield return new object[] { leadEntity, leads, leadMatchedDb, leadEntityDb, leadUpdateEntity, addLead, expected };

            //2. Случай, когда Лид восстанавливается (по совпадению по паспорту) и его акки остаются удаленными, но ему присваивается новый рублевый акк

            leadEntity = new LeadEntity
            {
                PassportNumber = "1111 00000027",
                Email = "127@mail.ru",
                PhoneNumber = "892100223227",
                Name = "127",
                Citizenship = "RUS",
                Accounts = new List<AccountEntity>
                    {
                        new AccountEntity
                        {
                            Id = 99,
                            LeadId = 307,
                            Status = AccountStatusEnum.Deleted,
                            Currency = "RUB"
                        },
                        new AccountEntity
                        {
                            Id = 100,
                            LeadId = 307,
                            Status = AccountStatusEnum.Deleted,
                            Currency = "USD"
                        },
                    }
            };
            leads = new List<LeadEntity>
            {
            new LeadEntity
                {
                Id = 307,
                PassportNumber = "1111 00000027",
                Email = "12307@mail.ru",
                PhoneNumber = "892100223307",
                Name = "12307",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Deleted,
                Role = LeadRoleEnum.StandardLead,
                Accounts = new List<AccountEntity>
                    {
                        new AccountEntity
                        {
                            Id = 99,
                            LeadId = 307,
                            Status = AccountStatusEnum.Deleted,
                            Currency = "RUB"
                        },
                        new AccountEntity
                        {
                            Id = 100,
                            LeadId = 307,
                            Status = AccountStatusEnum.Deleted,
                            Currency = "USD"
                        },
                    }
                }
            };
            leadMatchedDb = new Lead
            {
                Id = 307,
                PassportNumber = "1111 00000027",
                Email = "12307@mail.ru",
                PhoneNumber = "892100223307",
                Name = "12307",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Deleted,
                Role = LeadRoleEnum.StandardLead,
                Accounts = new List<Account>
                    {
                        new Account
                        {
                            Id = 99,
                            LeadId = 307,
                            Status = AccountStatusEnum.Deleted,
                            Currency = "RUB"
                        },
                        new Account
                        {
                            Id = 100,
                            LeadId = 307,
                            Status = AccountStatusEnum.Deleted,
                            Currency = "USD"
                        },
                    }
            };
            leadEntityDb = new LeadEntity
            {
                Id = 307,
                PassportNumber = "1111 00000027",
                Email = "12307@mail.ru",
                PhoneNumber = "892100223307",
                Name = "12307",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Deleted,
                Role = LeadRoleEnum.StandardLead,
                Accounts = new List<AccountEntity>
                    {
                        new AccountEntity
                        {
                            Id = 99,
                            LeadId = 307,
                            Status = AccountStatusEnum.Deleted,
                            Currency = "RUB"
                        },
                        new AccountEntity
                        {
                            Id = 100,
                            LeadId = 307,
                            Status = AccountStatusEnum.Deleted,
                            Currency = "USD"
                        },
                    }
            };
            leadUpdateEntity = new LeadEntity
            {
                Id = 307,
                PassportNumber = "1111 00000027",
                Email = "127@mail.ru",
                PhoneNumber = "892100223227",
                Name = "127",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Active,
                Role = LeadRoleEnum.StandardLead,
                Accounts = new List<AccountEntity>
                    {
                        new AccountEntity
                        {
                            Id = 130,
                            LeadId = 307,
                            Status = AccountStatusEnum.Active,
                            Currency = "RUB"
                        }
                    }
            };
            addLead = new Lead
            {
                PassportNumber = "1111 00000027",
                Email = "127@mail.ru",
                PhoneNumber = "892100223227",
                Name = "127",
                Citizenship = "RUS",
                Accounts = new List<Account>
                    {
                        new Account
                        {
                            Id = 99,
                            LeadId = 307,
                            Status = AccountStatusEnum.Deleted,
                            Currency = "RUB"
                        },
                        new Account
                        {
                            Id = 100,
                            LeadId = 307,
                            Status = AccountStatusEnum.Deleted,
                            Currency = "USD"
                        },
                    }
            };
            expected = new Lead
            {
                Id = 307,
                PassportNumber = "1111 00000027",
                Email = "127@mail.ru",
                PhoneNumber = "892100223227",
                Name = "127",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Active,
                Role = LeadRoleEnum.StandardLead,
                Accounts = new List<Account>
                    {
                        new Account
                        {
                            Id = 130,
                            LeadId = 307,
                            Status = AccountStatusEnum.Active,
                            Currency = "RUB"
                        }
                    }
            };

            yield return new object[] { leadEntity, leads, leadMatchedDb, leadEntityDb, leadUpdateEntity, addLead, expected };
        }

        public static IEnumerable CreateOrRecoverLeadAsyncTestCaseSource_WhenRecoverLeadWithSameEmail_ShouldBeAlreadyExistException()
        {
            // Случай, когда ВипЛид пытается создаться, но в базе уже есть такй емейл

            LeadEntity leadEntity = new LeadEntity
            {
                PassportNumber = "1111 00000029",
                Email = "129@mail.ru",
                PhoneNumber = "892100223229",
                Name = "129",
                Citizenship = "RUS"
            };
            List<LeadEntity> leads = new List<LeadEntity>
            {
            new LeadEntity
                {
                Id = 309,
                PassportNumber = "9999 88802",
                Email = "129@mail.ru",
                PhoneNumber = "892100223999",
                Name = "12309",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Deleted,
                Role = LeadRoleEnum.VipLead
                }
            };
            Lead leadMatchedDb = new Lead
            {
                Id = 309,
                PassportNumber = "9999 88802",
                Email = "129@mail.ru",
                PhoneNumber = "892100223999",
                Name = "12309",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Deleted,
                Role = LeadRoleEnum.VipLead
            };
            Lead addLead = new Lead
            {
                PassportNumber = "1111 00000029",
                Email = "129@mail.ru",
                PhoneNumber = "892100223229",
                Name = "129",
                Citizenship = "RUS"
            };

            yield return new object[] { leadEntity, leads, leadMatchedDb, addLead };
        }

        public static IEnumerable CreateOrRecoverLeadAsyncTestCaseSource_WhenRecoverLeadWithSameEmailAndPhoneNumber_ShouldBeAlreadyExistException()
        {
            // Случай, когда Лид пытается создаться, но в базе уже есть такй емейл и телефон

            LeadEntity leadEntity = new LeadEntity
            {
                PassportNumber = "1111 000000290",
                Email = "1290@mail.ru",
                PhoneNumber = "8921002232290",
                Name = "1290",
                Citizenship = "RUS"
            };
            List<LeadEntity> leads = new List<LeadEntity>
            {
            new LeadEntity
                {
                Id = 3090,
                PassportNumber = "9999 888020",
                Email = "1290@mail.ru",
                PhoneNumber = "8921002232290",
                Name = "123090",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Deleted,
                Role = LeadRoleEnum.StandardLead
                }
            };
            Lead leadMatchedDb = new Lead
            {
                Id = 3090,
                PassportNumber = "9999 888020",
                Email = "1290@mail.ru",
                PhoneNumber = "8921002232290",
                Name = "123090",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Deleted,
                Role = LeadRoleEnum.StandardLead
            };
            Lead addLead = new Lead
            {
                PassportNumber = "1111 000000290",
                Email = "1290@mail.ru",
                PhoneNumber = "8921002232290",
                Name = "1290",
                Citizenship = "RUS"
            };

            yield return new object[] { leadEntity, leads, leadMatchedDb, addLead };
        }

        public static IEnumerable CreateOrRecoverLeadAsyncTestCaseSource_WhenRecoverLeadWithSamePhoneNumber()
        {
            // Случай, когда Лид создается, но в базе уже есть удаленный лид с таким же телефоном

            LeadEntity leadEntity = new LeadEntity
            {
                PassportNumber = "11133290",
                Email = "12904@mail.ru",
                PhoneNumber = "892100224444",
                Name = "12904",
                Citizenship = "RUS"
            };
            List<LeadEntity> leads = new List<LeadEntity>
            {
            new LeadEntity
                {
                Id = 30904,
                PassportNumber = "111433290",
                Email = "12490@mail.ru",
                PhoneNumber = "892100224444",
                Name = "1230904",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Deleted,
                Role = LeadRoleEnum.StandardLead
                }
            };
            Lead leadMatchedDb = new Lead
            {
                Id = 30904,
                PassportNumber = "111433290",
                Email = "12490@mail.ru",
                PhoneNumber = "892100224444",
                Name = "1230904",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Deleted,
                Role = LeadRoleEnum.StandardLead
            };
            Lead addLead = new Lead
            {
                PassportNumber = "11133290",
                Email = "12904@mail.ru",
                PhoneNumber = "892100224444",
                Name = "12904",
                Citizenship = "RUS"
            };
            LeadEntity addLeadEntity = new LeadEntity
            {
                Id = 999,
                PassportNumber = "11133290",
                Email = "12904@mail.ru",
                PhoneNumber = "892100224444",
                Name = "12904",
                Citizenship = "RUS"
            };
            Lead expected = new Lead
            {
                Id = 999,
                PassportNumber = "11133290",
                Email = "12904@mail.ru",
                PhoneNumber = "892100224444",
                Name = "12904",
                Citizenship = "RUS"
            };

            yield return new object[] { leadEntity, leads, leadMatchedDb, addLead, addLeadEntity, expected };
        }

        public static IEnumerable CreateOrRecoverLeadAsyncTestCaseSource_WhenRecoverLeadAndStatusIsDeletedButRoleIsNotLead_ShouldBeArgumentException()
        {
            // Случай, когда Юзер(с ролью НеЛид) пытается создаться, но в базе уже есть такой удаленный - ожидаем ошибку

            LeadEntity leadEntity = new LeadEntity
            {
                PassportNumber = "111332901",
                Email = "129041@mail.ru",
                PhoneNumber = "8921002244441",
                Name = "129041",
                Citizenship = "RUS"
            };
            List<LeadEntity> leads = new List<LeadEntity>
            {
            new LeadEntity
                {
                Id = 309041,
                PassportNumber = "1114332901",
                Email = "129041@mail.ru",
                PhoneNumber = "8921001224444",
                Name = "12309041",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Deleted,
                Role = LeadRoleEnum.Manager
                }
            };
            Lead leadMatchedDb = new Lead
            {
                Id = 309041,
                PassportNumber = "1114332901",
                Email = "129041@mail.ru",
                PhoneNumber = "8921001224444",
                Name = "12309041",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Deleted,
                Role = LeadRoleEnum.Manager
            };
            Lead addLead = new Lead
            {
                PassportNumber = "111332901",
                Email = "129041@mail.ru",
                PhoneNumber = "8921002244441",
                Name = "129041",
                Citizenship = "RUS"
            };

            yield return new object[] { leadEntity, leads, leadMatchedDb, addLead };
        }

        public static IEnumerable CreateOrRecoverLeadAsyncTestCaseSource_WhenRecoverLeadAndStatusIsActive_ShouldBeAlreadyExistException()
        {
            // Случай, когда нашелся в базе Юзер с таким же номером телефона и он активный - ожидаем ошибку

            LeadEntity leadEntity = new LeadEntity
            {
                PassportNumber = "1113329015",
                Email = "129041251@mail.ru",
                PhoneNumber = "87451120000",
                Name = "1290415",
                Citizenship = "RUS"
            };
            List<LeadEntity> leads = new List<LeadEntity>
            {
            new LeadEntity
                {
                Id = 3090415,
                PassportNumber = "11143329015",
                Email = "12904125@mail.ru",
                PhoneNumber = "87451120000",
                Name = "123090415",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Active,
                Role = LeadRoleEnum.StandardLead
                }
            };
            Lead leadMatchedDb = new Lead
            {
                Id = 3090415,
                PassportNumber = "11143329015",
                Email = "12904125@mail.ru",
                PhoneNumber = "87451120000",
                Name = "123090415",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Active,
                Role = LeadRoleEnum.StandardLead
            };
            Lead addLead = new Lead
            {
                PassportNumber = "1113329015",
                Email = "129041251@mail.ru",
                PhoneNumber = "87451120000",
                Name = "1290415",
                Citizenship = "RUS"
            };

            yield return new object[] { leadEntity, leads, leadMatchedDb, addLead };
        }

        public static IEnumerable CreateOrRecoverLeadAsyncTestCaseSource_WhenInDbTwoMatchesWithReqestLesd_ShouldBeArgumentException()
        {
            // Случай, когда нашлся в базе два Юзера - 1 с таким же номером телефона, 2 - с таким же емейлом - ожидаем ошибку

            LeadEntity leadEntity = new LeadEntity
            {
                PassportNumber = "4",
                Email = "4@mail.ru",
                PhoneNumber = "444442222",
                Name = "129041544",
                Citizenship = "RUS"
            };
            List<LeadEntity> leads = new List<LeadEntity>
            {
            new LeadEntity
                {
                Id = 30904154,
                PassportNumber = "111433290154",
                Email = "129041254@mail.ru",
                PhoneNumber = "444442222",
                Name = "1230904154",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Deleted,
                Role = LeadRoleEnum.StandardLead
                },
            new LeadEntity
                {
                Id = 6,
                PassportNumber = "666666666",
                Email = "4@mail.ru",
                PhoneNumber = "444442222666",
                Name = "12309041546666",
                Citizenship = "RUS",
                Status = LeadStatusEnum.Deleted,
                Role = LeadRoleEnum.StandardLead
                },
            };
            int leadMatchedId1 = 30904154;
            int leadMatchedId2 = 6;
            Lead addLead = new Lead
            {
                PassportNumber = "4",
                Email = "4@mail.ru",
                PhoneNumber = "444442222",
                Name = "129041544",
                Citizenship = "RUS"
            };

            yield return new object[] { leadEntity, leads, addLead, leadMatchedId1, leadMatchedId2 };
        }
    }
}