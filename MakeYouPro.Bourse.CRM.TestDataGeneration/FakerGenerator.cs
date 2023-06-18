using Bogus;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Dal.Models;

namespace MakeYouPro.Bourse.CRM.TestDataGeneration
{
    public class FakerGenerator
    {
        private const string DefaultCurrency = "RUB";
        private readonly Random _random = new Random();
        private readonly Faker<AccountEntity> _accountFaker;
        private readonly Faker<LeadEntity> _leadFaker;
        //это я добавил
        private readonly List<AccountStatusEnum> _accountStatuses = Enum.GetValues(typeof(AccountStatusEnum)).Cast<AccountStatusEnum>().ToList();
        private readonly List<LeadStatusEnum> _leadStatuses = Enum.GetValues(typeof(LeadStatusEnum)).Cast<LeadStatusEnum>().ToList();
        private  int _lastIndex = 0;
        //

        private readonly Dictionary<LeadRoleEnum, HashSet<string>> _roleToCurrencies = new()
        {
            {
                LeadRoleEnum.VipLead,  new HashSet<string>
                {
                    "RUB",
                    "USD",
                    "EUR",
                    "PY",
                    "CNY",
                    "RSD",
                    "BGN",
                    "ARS"
                }
            },
            {
                LeadRoleEnum.StandardLead,  new HashSet<string>
                {
                    "RUB",
                    "USD",
                    "EUR"
                }
            }
        };

        private LeadRoleEnum _currentLeadRole;
        private LeadEntity _currentLead;
        private HashSet<string> _currentCurrencies = new();

        public FakerGenerator()
        {
            _accountFaker = GetAccountFaker();
            _leadFaker = GetLeadFaker();
        }

        public List<LeadEntity> GenerateLeads(int count, LeadRoleEnum role)
        {
            _currentLeadRole = role;
            return _leadFaker.Generate(count);
        }

        public List<AccountEntity> GenerateAccountsForLeads(IEnumerable<LeadEntity> leads)
        {
            var result = new List<AccountEntity>();

            foreach (var lead in leads)
            {
                //
                //
                _currentLead = lead;
                _currentCurrencies = new HashSet<string>(_roleToCurrencies[_currentLeadRole]);
                var accCount = _random.Next(1, _currentCurrencies.Count);
                //и это добавил
                _currentLeadRole = lead.Role;
                var accounts = _accountFaker.Generate(accCount);
                if (lead.Status == LeadStatusEnum.Deleted)
                {
                    accounts.FindAll(a => a.Status == AccountStatusEnum.Active)
                        .Select(a => a.Status = AccountStatusEnum.Deleted);
                }
                //
                result.AddRange(accounts);
            }
            return result;
        }

        private Faker<LeadEntity> GetLeadFaker()
        {
            return new Faker<LeadEntity>("ru")
                .RuleFor(x => x.Id, f => _lastIndex+=1)
                //.RuleFor(x => x.Status, f => LeadStatusEnum.Active)
                .RuleFor(x => x.Status, f => f.Random.ListItem(_leadStatuses))
                .RuleFor(x => x.DateCreate, f => f.Date.Between(new DateTime(2019, 01, 01), new DateTime(2021, 01, 01)))
                .RuleFor(x => x.Name, f => f.Name.FirstName())
                .RuleFor(x => x.MiddleName, f => f.Name.LastName())
                .RuleFor(x => x.Surname, f => f.Name.LastName())
                .RuleFor(x => x.Birthday, f => DateOnly.FromDateTime(f.Date.Between(new DateTime(1950, 01, 01), new DateTime(2000, 01, 01))))
                .RuleFor(x => x.Email,(f,x) => $"{x.Id}@gmail.com")
                .RuleFor(x => x.Citizenship, f => f.Random.ListItem(new List<string>
                {
                    //мб другие страны засунем?
                    "RUS",
                    "USA",
                    "JPN",
                    "POL",
                    "GRC"
                    //
                }))
                .RuleFor(x => x.PassportNumber, f => f.Random.AlphaNumeric(10))
                .RuleFor(x => x.PhoneNumber, f => f.Random.Digits(11, 0, 9).ToString())
                .RuleFor(x => x.Registration, f => f.Random.RandomLocale())
                .RuleFor(x => x.Comment, f => f.Random.Words(1))
                .RuleFor(x => x.Role, f => _currentLeadRole);
        }

        private Faker<AccountEntity> GetAccountFaker()
        {
            return new Faker<AccountEntity>("ru")
                .RuleFor(x => x.LeadId, f => _currentLead.Id)
                .RuleFor(x => x.DateCreate, f =>
                {
                    if (_currentCurrencies.Contains(DefaultCurrency))
                    {
                        return _currentLead.DateCreate;
                    }
                    else
                    {
                        return f.Date.Between(_currentLead.DateCreate, new DateTime(2023, 06, 01));
                    }
                })
                //.RuleFor(x => x.Status, f => AccountStatusEnum.Active)
                .RuleFor(x => x.Status, f => f.Random.ListItem(_accountStatuses))
                .RuleFor(x => x.Comment, f => f.Random.Words(1))
                .RuleFor(x => x.Currency, f =>
                {
                    if (_currentCurrencies.Remove(DefaultCurrency))
                    {
                        return DefaultCurrency;
                    }
                    var currency = f.Random.CollectionItem(_currentCurrencies);
                    _currentCurrencies.Remove(currency);
                    return currency;
                });
            //.RuleFor(x => x.Status, (f, x) =>
            //{
            //    if (_statusCurrentLead == LeadStatusEnum.Deleted &&
            //    x.Status == AccountStatusEnum.Active)
            //    {
            //        x.Status = AccountStatusEnum.Deleted;
            //    }
            //});
        }
    }
}
