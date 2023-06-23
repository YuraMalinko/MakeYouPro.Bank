using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Account.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Transaction.Request;
using MakeYouPro.Bourse.CRM.Api.Validations;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.Services;
using MakeYouPro.Bourse.CRM.Core.Configurations.ISettings;
using MakeYouPro.Bourse.CRM.Core.Configurations.Settings;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using MakeYouPro.Bourse.CRM.Dal.Repositories;

namespace MakeYouPro.Bourse.CRM.Api.Extentions
{
    public static class InjectionExtentions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ILeadRepository, LeadRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
        }
        
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ILeadService, LeadService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITransactionService, TransactionService>();
        }

        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<AccountCreateRequest>, CreateAccountValidator>();
            services.AddScoped<IValidator<AccountFilterRequest>, AccountFilterValidation>();
            services.AddScoped<IValidator<CreateLeadRequest>, RegistrateValidator>();
            services.AddScoped<IValidator<TransactionRequest>, TransactionValidator>();
        }

        public static void AddSettings(this IServiceCollection services)
        {
            services.AddScoped<ICurrencySetting, CurrencySetting>();
            services.AddScoped<IAccountSetting, AccountSetting>();
        }
    }
}
