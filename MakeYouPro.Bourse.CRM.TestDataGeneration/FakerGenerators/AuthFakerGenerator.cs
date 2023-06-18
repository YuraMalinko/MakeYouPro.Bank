using Bogus;
using MakeYouPro.Bourse.CRM.Auth.Dal.Models;
using MakeYouPro.Bourse.CRM.Dal.Models;
using MakeYouPro.Bourse.CRM.TestDataGeneration.Services;

namespace MakeYouPro.Bourse.CRM.TestDataGeneration.FakerGenerators
{
    internal class AuthFakerGenerator
    {
        private readonly Faker<UserEntity> _userFaker;
        private LeadEntity _currentLead;
        private WhriterToFile _whriter = new WhriterToFile();

        public AuthFakerGenerator()
        {
            _userFaker = GetUserFaker();
        }

        public List<UserEntity> GenerateUsers(IEnumerable<LeadEntity> leads)
        {
            var result = new List<UserEntity>();

            foreach (var l in leads)
            {
                _currentLead = l;
                var user = _userFaker.Generate();
                result.Add(user);
            }

            _whriter.WhriteUsers(result);
            foreach (var r in result)
            {
                var password = BCrypt.Net.BCrypt.HashPassword(r.Password);
                r.Password=password;
            }

            return result;
        }

        private Faker<UserEntity> GetUserFaker()
        {
            return new Faker<UserEntity>("en")
                .RuleFor(u => u.Id, f => _currentLead.Id)
                .RuleFor(u => u.Email, f => _currentLead.Email)
                .RuleFor(u => u.Password, f => f.Internet.Password(4))
                .RuleFor(u => u.Role, f => _currentLead.Role)
                .RuleFor(u => u.Status, f => _currentLead.Status);
        }
    }
}