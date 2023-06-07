using MakeYouPro.Bourse.CRM.Api.Injections;
using MakeYouPro.Bourse.CRM.Api.Mappings;
using MakeYouPro.Bourse.CRM.Bll.Mappings;
using MakeYouPro.Bourse.CRM.Core.Clients.AuthService;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using NLog;
using ILogger = NLog.ILogger;
using LogManager = NLog.LogManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
var nlog = LogManager.Setup().GetCurrentClassLogger();
builder.Services.AddSingleton<ILogger>(nlog);

builder.Services.AddAutoMapper(typeof(MapperApiLeadProfile), typeof(MapperBllLeadProfile),
    typeof(MapperApiAccountProfile), typeof(MapperBllAccountProfile));
new InjectionSettings(builder);
new InjectionRepositoriesAndContext(builder);
new InjectionServices(builder);
new InjectionValidators(builder);

builder.Services.AddScoped<IAuthServiceClient, AuthServiceClient>(_ => new AuthServiceClient(Environment.GetEnvironmentVariable("AuthServiceUrl")));



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();