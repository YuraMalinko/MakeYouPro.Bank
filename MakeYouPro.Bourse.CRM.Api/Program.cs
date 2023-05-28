using MakeYouPro.Bourse.CRM.Api;
using NLog;
using ILogger = NLog.ILogger;
using LogManager = NLog.LogManager;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
var nlog = LogManager.Setup().GetCurrentClassLogger();
builder.Services.AddSingleton<ILogger>(nlog);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

new InjectionSettings(builder); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();