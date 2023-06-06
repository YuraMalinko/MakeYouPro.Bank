using MakeYouPro.Bourse.CRM.Dal;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using MakeYouPro.Bourse.CRM.Dal.Repositories;

namespace MakeYouPro.Bourse.CRM.Api.Injections
{
    public class InjectionRepositories
    {
        public InjectionRepositories(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<CRMContext>();
            builder.Services.AddScoped<ILeadRepository, LeadRepository>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
        }
    }
}
