﻿using MakeYouPro.Bourse.CRM.Dal.Models;
using Bogus;
using MakeYouPro.Bourse.CRM.Core.Enums;

namespace MakeYouPro.Bourse.CRM.TestDataGeneration
{
    public class FakerGenerator
    {
        private const string DefaultCurrency = "RUB";
        private readonly Random _random = new Random();
        private readonly Faker<AccountEntity> _accountFaker;
        private readonly Faker<LeadEntity> _leadFaker;
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

        public List<AccountEntity> GenerateAccountsForLeads(List<LeadEntity> leads)
        {
            var result = new List<AccountEntity>();

            foreach (var lead in leads)
            {
                _currentLeadRole = lead.Role;
                _currentLead = lead;
                _currentCurrencies = new HashSet<string>(_roleToCurrencies[_currentLeadRole]);
                var accCount = _random.Next(1, _currentCurrencies.Count);
                result.AddRange(_accountFaker.Generate(accCount));
            }
            return result;
        }

        private Faker<LeadEntity> GetLeadFaker()
        {
            return new Faker<LeadEntity>("ru")
                .RuleFor(x => x.Status, f => LeadStatusEnum.Active)
                .RuleFor(x => x.DateCreate, f => f.Date.Between(new DateTime(2019, 01, 01), new DateTime(2021, 01, 01)))
                .RuleFor(x => x.Name, f => f.Name.FirstName())
                .RuleFor(x => x.MiddleName, f => f.Name.LastName())
                .RuleFor(x => x.Surname, f => f.Name.LastName())
                .RuleFor(x => x.Birthday, f => DateOnly.FromDateTime(f.Date.Between(new DateTime(1950, 01, 01), new DateTime(2000, 01, 01))))
                .RuleFor(x => x.Email, f => $"{f.IndexFaker}@gmail.com")
                .RuleFor(x => x.Citizenship, f => f.Random.ListItem(new List<string>
                {
                    "RU",
                    "USA",
                    "J",
                    "PL",
                    "GR"
                }))
                .RuleFor(x => x.PassportNumber, f => f.Random.AlphaNumeric(10))
                .RuleFor(x => x.PhoneNumber, f => f.Random.Digits(11, 0, 9).ToString())
                .RuleFor(x => x.Registration, f => f.Random.Words(15))
                .RuleFor(x => x.Comment, f => f.Random.Words(15))
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
                .RuleFor(x => x.Status, f => AccountStatusEnum.Active)
                .RuleFor(x => x.Comment, f => f.Random.Words(10))
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
        }
    }
}
