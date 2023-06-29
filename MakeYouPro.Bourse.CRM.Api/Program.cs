using MakeYouPro.Bourse.CRM.Api.Extentions;
using MakeYouPro.Bourse.CRM.Api.Mappings;
using MakeYouPro.Bourse.CRM.Bll.Mappings;
using MakeYouPro.Bourse.CRM.Bll.RabbitMQ.Models;
using MakeYouPro.Bourse.CRM.Core.Clients.AuthService;
using MakeYouPro.Bourse.CRM.Core.Clients.TransactionService;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;
using MakeYouPro.Bourse.CRM.Core.RabbitMQ;
using MakeYouPro.Bourse.CRM.Dal;
using NLog;
using ILogger = NLog.ILogger;
using LogManager = NLog.LogManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
var nlog = LogManager.Setup().GetCurrentClassLogger();
builder.Services.AddSingleton<ILogger>(nlog);

builder.Services.AddAutoMapper(typeof(MapperApiLeadProfile), typeof(MapperBllLeadProfile),
                                typeof(MapperApiAccountProfile), typeof(MapperBllAccountProfile),
                                typeof(MapperApiTransactionProfile), typeof(MapperClientTransactionProfile));

builder.Services.AddTransient<CRMContext>(_ => new CRMContext(Environment.GetEnvironmentVariable("EncryptKey")));
builder.Services.AddRepositories();

builder.Services.AddServices();

builder.Services.AddValidators();

builder.Services.AddSettings();

builder.Services.AddRabbitMQ();

builder.Services.AddScoped<IAuthServiceClient, AuthServiceClient>(_ => new AuthServiceClient(Environment.GetEnvironmentVariable("AuthServiceUrl")));
builder.Services.AddScoped<ITransactionServiceClient, TransactionServiceClient>(_ => new TransactionServiceClient(Environment.GetEnvironmentVariable("TransactionServiceUrl")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}
app.UseMiddleware<ExceptionHandler>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Services.GetRequiredService<IConsumer<UpdateRoleMessage>>();

app.Run();