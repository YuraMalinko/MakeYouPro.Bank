using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MakeYouPro.Bource.CRM.Dal;
using MakeYouPro.Bourse.CRM.Api.Mappings;
using MakeYouPro.Bourse.CRM.Bll.Mappings;
using MakeYouPro.Bourse.CRM.Bll.IServices;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using MakeYouPro.Bourse.CRM.Bll.Services;
using MakeYouPro.Bourse.CRM.Dal.Repositories;
using NLog;
using ILogger = NLog.ILogger;
using LogManager = NLog.LogManager;
using MakeYouPro.Bourse.CRM.Core.ExceptionMiddleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nLog.config"));
var nLog = LogManager.Setup().GetCurrentClassLogger();
builder.Services.AddSingleton<ILogger>(nLog);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<CRMContext>();
builder.Services.AddAutoMapper(typeof(MapperApiLeadProfile), typeof(MapperBllLeadProfile),
                               typeof(MapperApiAccountProfile), typeof(MapperBllAccountProfile));
builder.Services.AddScoped<ILeadService, LeadService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ILeadRepository, LeadRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ExceptionHandler>();

app.MapControllers();

app.Run();
