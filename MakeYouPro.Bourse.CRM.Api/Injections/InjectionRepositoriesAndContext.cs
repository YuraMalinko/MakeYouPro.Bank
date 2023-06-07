using MakeYouPro.Bourse.CRM.Dal;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using MakeYouPro.Bourse.CRM.Dal.Repositories;

namespace MakeYouPro.Bourse.CRM.Api.Injections
{
    public class InjectionRepositoriesAndContext
    {
        public InjectionRepositoriesAndContext(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<CRMContext>(_ => new CRMContext(Environment.GetEnvironmentVariable("EncryptKey")));
            builder.Services.AddScoped<ILeadRepository, LeadRepository>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        }
    }
}
