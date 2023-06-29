using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Account.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Transaction.Request;
using MakeYouPro.Bourse.CRM.Api.Validations;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.RabbitMQ;
using MakeYouPro.Bourse.CRM.Bll.RabbitMQ.Models;
using MakeYouPro.Bourse.CRM.Bll.Services;
using MakeYouPro.Bourse.CRM.Core.Configurations.ISettings;
using MakeYouPro.Bourse.CRM.Core.Configurations.Settings;
using MakeYouPro.Bourse.CRM.Core.RabbitMQ;
using MakeYouPro.Bourse.CRM.Core.RabbitMQ.Models;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using MakeYouPro.Bourse.CRM.Dal.Repositories;
using RabbitMQ.Client;

namespace MakeYouPro.Bourse.CRM.Api.Extentions
{
    public static class InjectionExtentions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ILeadRepository, LeadRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ILeadService, LeadService>();
            //services.AddSingleton<ILeadSingletoneService, LeadService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITransactionService, TransactionService>();
        }

        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<AccountCreateRequest>, CreateAccountValidator>();
            services.AddScoped<IValidator<AccountFilterRequest>, AccountFilterValidation>();
            services.AddScoped<IValidator<CreateLeadRequest>, RegistrateValidator>();
            services.AddScoped<IValidator<TransactionRequest>, TransactionValidator>();
            services.AddScoped<IValidator<TransferTransactionRequest>, TransferTransactionValidator>();
            services.AddScoped<IValidator<UpdateLeadUsingLeadRequest>, UpdateUsingLeadValidator>();
            services.AddScoped<IValidator<UpdateLeadUsingManagerRequest>, UpdateUsingManagerValidator>();
        }

        public static void AddSettings(this IServiceCollection services)
        {
            services.AddSingleton<ICurrencySetting, CurrencySetting>();
            services.AddSingleton<IAccountSetting, AccountSetting>();
            services.AddSingleton<ICommissionSettings, CommissionSettings>();
        }

        public static void AddRabbitMQ(this IServiceCollection services)
        {
            services.AddSingleton(_ => new ConnectionFactory { HostName = Environment.GetEnvironmentVariable("RabbitHostName") }.CreateConnection());

            services.AddSingleton<IProduser<CommissionMessage>, Produser<CommissionMessage>>(
                s => new Produser<CommissionMessage>(s.GetRequiredService<IConnection>(), "commissionExchange", "commissionQueue"));
            services.AddSingleton<IConsumer<UpdateRoleMessage>, UpdateRoleConsumer>(
                s => new UpdateRoleConsumer(s.GetRequiredService<IConnection>(), "update-lead-status-on-vip",
                                            s.GetRequiredService<ILeadRepository>(), s.GetRequiredService<NLog.ILogger>()));
            //services.AddHostedService<UpdateRoleConsumer>(
            //    s => new UpdateRoleConsumer(s.GetRequiredService<IConnection>(), "update-lead-status-on-vip",
            //                                s.GetRequiredService<ILeadService>(), s.GetRequiredService<NLog.ILogger>()));
        }
    }
}