using ReportingService.Api.RabbitMQ;
using ReportingService.Bll;
using ReportingService.Bll.Services;
using ReportingService.Dal;
using ReportingService.Dal.IRepository.CRM;
using ReportingService.Dal.Repository.CRM;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRabbitMqService, RabbitMqService>();
//builder.Services.AddHostedService<RabbitMqListener>();
builder.Services.AddSingleton<IRecordingServices, RecordingServices>();
builder.Services.AddSingleton<IMessageHandler, MessageHandler>();
builder.Services.AddSingleton<ILeadRepository, LeadRepository>();
builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
builder.Services.AddSingleton<Context>();
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<RabbitMqListener>();
    })
    .Build();

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
