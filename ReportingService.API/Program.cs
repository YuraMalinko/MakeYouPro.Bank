using ReportingService.Api.FinalRabbitMQ.MessageHandler;
using ReportingService.Api.InternetRabbitMQ;
using ReportingService.Api.MessageBroker;
using ReportingService.Api.MessageBroker.Configuration;
using ReportingService.Api.MessageBroker.Handlers;
using ReportingService.Api.MessageBroker.Interfaces;
using ReportingService.Api.MessageBroker.Serializer;
using ReportingService.Bll;
using ReportingService.Bll.IServices;
using ReportingService.Bll.Services;
using ReportingService.Dal;
using ReportingService.Dal.IRepository.CRM;
using ReportingService.Dal.Repository.CRM;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddScoped<IRabbitMqServicetest, RabbitMqServicetest>();
////builder.Services.AddHostedService<RabbitMqListener>();
//builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();
//builder.Services.AddSingleton<IConsumerService, ConsumerService>();
//builder.Services.AddHostedService<ConsumerHostedService>();

// inject configuration for Listener and service that uses Publisher
var rabbitMqPublisherSettings = builder.Configuration.GetSection("RabbitMqConfiguration").Get<RabbitMqSettings>();
var leadServiceSettings = builder.Configuration.GetSection("LeadServiceConfiguration").Get<RouteServiceSettings>();
builder.Services.AddSingleton(rabbitMqPublisherSettings);
builder.Services.AddSingleton(leadServiceSettings);

// Inject Publisher dependencies
builder.Services.AddScoped<ISerializer, JsonSerializer>();
builder.Services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();

// Inject Listener dependencies
builder.Services.AddSingleton<IHandlerFactory, HandlerFactory>();
builder.Services.AddHostedService<RabbitMqListener>();

//builder.Services.AddSingleton<IRabbitMqListener, RabbitMqListener>();
builder.Services.AddSingleton<IRecordingServices, RecordingServices>();
builder.Services.AddSingleton<IMessageHandler, MessageHandler>();
builder.Services.AddSingleton<ILeadRepository, LeadRepository>();
builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
builder.Services.AddSingleton<Context>();

//builder.Services.AddAutoMappe
//IHost host = Host.CreateDefaultBuilder(args)
//    .ConfigureServices(services =>
//    {
//        services.AddHostedService<RabbitMqListener>();
//    })
//    .Build();

var app = builder.Build();

// Add handlers to factory
CreateFactory(app);

//host.Start();

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

void CreateFactory(WebApplication app)
{
    var factory = app.Services.GetRequiredService<IHandlerFactory>();
    factory.AddHandler("Create", new CreateLeadHandler(app.Services.GetRequiredService<IRecordingServices>()));
    factory.AddHandler("Update", new UpdateLeadHandler(app.Services.GetRequiredService<IRecordingServices>()));
}