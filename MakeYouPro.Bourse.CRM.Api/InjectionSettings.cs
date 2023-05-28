using MakeYouPro.Bourse.CRM.Dal;
using MakeYouPro.Bourse.CRM.Api.Mappings;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Mappings;
using MakeYouPro.Bourse.CRM.Bll.Services;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using MakeYouPro.Bourse.CRM.Dal.Repositories;

namespace MakeYouPro.Bourse.CRM.Api
{
    public class InjectionSettings
    {
        public InjectionSettings(WebApplicationBuilder? builder)
        {
            builder.Services.AddAutoMapper(typeof(MapperApiLeadProfile), typeof(MapperBllLeadProfile),
                typeof(MapperApiAccountProfile), typeof(MapperBllAccountProfile));

            builder.Services.AddScoped<CRMContext>();

            builder.Services.AddScoped<ILeadRepository, LeadRepository>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();

            builder.Services.AddScoped<ILeadService, LeadService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
        }
    }
}
