using FluentValidation;
using MakeYouPro.Bourse.CRM.Api.Models.Account.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Lead.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Transaction.Request;
using MakeYouPro.Bourse.CRM.Api.Models.Users.Request;
using MakeYouPro.Bourse.CRM.Api.Validations;
using MakeYouPro.Bourse.CRM.Auth.Bll.IServices;
using MakeYouPro.Bourse.CRM.Auth.Bll.Models;
using MakeYouPro.Bourse.CRM.Auth.Dal.Context;
using MakeYouPro.Bourse.CRM.Auth.Dal.IRepository;
using MakeYouPro.Bourse.CRM.Auth.Dal.Repository;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Bll.RabbitMQ;
using MakeYouPro.Bourse.CRM.Bll.RabbitMQ.Models;
using MakeYouPro.Bourse.CRM.Bll.Services;
using MakeYouPro.Bourse.CRM.Core.Configurations.ISettings;
using MakeYouPro.Bourse.CRM.Core.Configurations.Settings;
using MakeYouPro.Bourse.CRM.Core.Enums;
using MakeYouPro.Bourse.CRM.Core.RabbitMQ;
using MakeYouPro.Bourse.CRM.Core.RabbitMQ.Models;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using MakeYouPro.Bourse.CRM.Dal.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace MakeYouPro.Bourse.CRM.Api.Extentions
{
    public static class InjectionExtentions
    {
        public static void AddRepositories(this IServiceCollection services)
        {

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddTransient<ILeadRepository, LeadRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ILeadService, LeadService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IAuthService, AuthService>();
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
            services.AddScoped<IValidator<UserUpdateRequest>, UpdatePasswordValidatior>();
        }

        public static void AddSettings(this IServiceCollection services)
        {
            services.AddSingleton<ICurrencySetting, CurrencySetting>();
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
        }

        public static void AddAuth(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(jwt =>
                {

                    var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("PrivateKey")!);
                    jwt.SaveToken = true;

                    jwt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),

                        // Just to avoid issues on localhost, it must be true on prod
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        // To avoid re-generation scenario just for develop
                        RequireExpirationTime = false,

                        ValidateLifetime = true
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("StandartLead", builder =>
                {
                    builder.RequireAssertion(k => k.User.HasClaim(ClaimTypes.Role, LeadRoleEnum.StandartLead.ToString()));
                });

                options.AddPolicy("VipLead", builder =>
                {
                    builder.RequireAssertion(k => k.User.HasClaim(ClaimTypes.Role, LeadRoleEnum.VipLead.ToString())
                        || k.User.HasClaim(ClaimTypes.Role, LeadRoleEnum.StandartLead.ToString()));
                });

                options.AddPolicy("Manager", builder =>
                {
                    builder.RequireAssertion(k => k.User.HasClaim(ClaimTypes.Role, LeadRoleEnum.Manager.ToString()));

                });

            });

            services
                .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<UserContext>();

            services
                .AddControllersWithViews()
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        }
    }
}