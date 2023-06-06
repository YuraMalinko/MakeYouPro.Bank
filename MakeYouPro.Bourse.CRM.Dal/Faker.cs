using Bogus;
using MakeYouPro.Bource.CRM.Core.Enums;
using MakeYouPro.Bource.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeYouPro.Bourse.CRM.Dal
{
    // Faker faker = new Faker("ru");

    public class FakerGenerate
    {
        public static Faker<LeadEntity> GetGeneratorRandomLead()
        {
            return new Faker<LeadEntity>("ru")
                .RuleFor(x => x.Id, f => f.IndexFaker)
                .RuleFor(x => x.Role, f => f.Random.ListItem(new List<LeadRoleEnum>
                {
                LeadRoleEnum.StandardLead, LeadRoleEnum.VipLead, LeadRoleEnum.Manager
                }))
                .RuleFor(x => x.Status, f => f.Random.ListItem(new List<LeadStatusEnum>
                {
                LeadStatusEnum.Active, LeadStatusEnum.Deactive
                }))
                .RuleFor(x => x.DateCreate, f => f.Date.Between(new DateTime(2021, 01, 01), new DateTime(2023, 06, 01)))
                .RuleFor(x => x.Name, f => f.Name.FirstName())
                .RuleFor(x => x.MiddleName, f => f.Name.Prefix())
                .RuleFor(x => x.Surname, f => f.Name.LastName())
                .RuleFor(x => x.PhoneNumber, f => f.Phone.PhoneNumberFormat())
                .RuleFor(x => x.Email, f => f.Internet.Email())
                .RuleFor(x => x.Citizenship, f => f.Random.ListItem(new List<string>
                {
                "RU",
                "USA",
                "BLR"
                }))
                .RuleFor(x => x.PassportNumber, f => f.Random.AlphaNumeric(10))
                .RuleFor(x => x.Registration, f => f.Random.Words(15))
                .RuleFor(x => x.Comment, f => f.Random.Words(15))
                .RuleFor(x => x.IsDeleted, f => f.Random.Bool());
            //.RuleFor(x => x.Accounts, f =>
            //{
            //    return new List<AccountEntity>
            //{
            //new AccountEntity
            //{

            //}
            //}
            //})
        }

        public static Faker<AccountEntity> GetGeneratorRandomAccount()
        {
            return new Faker<AccountEntity>("ru")
                .RuleFor(x => x.Id, f => f.IndexFaker)
                .RuleFor(x => x.LeadId, f => f.IndexFaker)
                .RuleFor(x => x.DateCreate, f => f.Date.Between(new DateTime(2021, 01, 01), new DateTime(2023, 06, 01)))
                .RuleFor(x => x.Currency, f => f.Random.ListItem(new List<string>
                {
                "RUB",
                "USD",
                "EUR"
                }))
                .RuleFor(x => x.Balance, f => f.Random.Decimal())
                .RuleFor(x => x.Status, f => f.PickRandom<AccountStatusEnum>())
                .RuleFor(x => x.Comment, f => f.Random.Words(15))
                .RuleFor(x => x.IsDeleted, f => f.Random.Bool());
        }




    }

}
