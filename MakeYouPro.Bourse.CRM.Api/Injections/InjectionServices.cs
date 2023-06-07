using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Services;
using MakeYouPro.Bourse.CRM.Core.Clients.AuthService;

namespace MakeYouPro.Bourse.CRM.Api.Injections
{
    public class InjectionServices
    {
        public InjectionServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ILeadService, LeadService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
        }
    }
}
