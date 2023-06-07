using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Account.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;
using MakeYouPro.Bourse.CRM.Api.Validations;

namespace MakeYouPro.Bourse.CRM.Api.Injections
{
    public class InjectionValidators
    {
        public InjectionValidators(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IValidator<AccountCreateRequest>, CreateAccountValidator>();
            builder.Services.AddScoped<IValidator<AccountFilterRequest>, AccountFilterValidation>();
            builder.Services.AddScoped<IValidator<CreateLeadRequest>, RegistrateValidator>();
        }
    }
}
