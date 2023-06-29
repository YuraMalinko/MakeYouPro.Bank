using CoreRS.CustomExceptionMiddleware;
using CoreRS.Logger;
using NLog;
using ReportingService.Api.MessageBroker;
using ReportingService.Api.MessageBroker.Configuration;
using ReportingService.Api.MessageBroker.Handlers;
using ReportingService.Api.MessageBroker.Interfaces;
using ReportingService.Api.MessageBroker.Serializer;
using ReportingService.Api.MessageHandler;
using ReportingService.Bll;
using ReportingService.Bll.IServices;
using ReportingService.Bll.Services;
using ReportingService.Dal;
using ReportingService.Dal.IRepository;
using ReportingService.Dal.IRepository.CRM;
using ReportingService.Dal.Repository.CRM;
using ReportingService.Dal.Repository.TransactionStore;

var builder = WebApplication.CreateBuilder(args);

LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// inject configuration for Listener and service that uses Publisher
var rabbitMqPublisherSettings = builder.Configuration.GetSection("RabbitMqConfiguration").Get<RabbitMqSettings>();
var leadServiceSettings = builder.Configuration.GetSection("RouteServiceConfiguration").Get<RouteServiceSettings>();
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
builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();
builder.Services.AddSingleton<Context>();
builder.Services.AddAutoMapper(typeof(MapperBLL));

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

app.UseMiddleware<ExceptionMiddleware>();

app.Run();

void CreateFactory(WebApplication app)
{
    var factory = app.Services.GetRequiredService<IHandlerFactory>();
    factory.AddHandler("CreateLead", new CreateLeadHandler(app.Services.GetRequiredService<IRecordingServices>()));
    factory.AddHandler("UpdateLead", new UpdateLeadHandler(app.Services.GetRequiredService<IRecordingServices>()));
    factory.AddHandler("CreateAccount", new CreateAccountHandler(app.Services.GetRequiredService<IRecordingServices>()));
    factory.AddHandler("UpdateAccount", new UpdateAccountHandler(app.Services.GetRequiredService<IRecordingServices>()));
}