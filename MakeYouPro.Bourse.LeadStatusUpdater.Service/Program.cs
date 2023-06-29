using MakeYouPro.Bourse.LeadStatusUpdater.Service;
using MakeYouPro.Bourse.LeadStatusUpdater.Service.RabbitMq;
using MakeYouPro.Bourse.LeadStatusUpdater.Service.RabbitMq.Configuration;
using MakeYouPro.Bourse.LeadStatusUpdater.Service.RabbitMq.Interfaces;
using MakeYouPro.Bourse.LeadStatusUpdater.Service.RabbitMq.Serializer;
using NLog;
using ILogger = NLog.ILogger;
using LogManager = NLog.LogManager;

LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
var nlog = LogManager.Setup().GetCurrentClassLogger();

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<RabbitMqPublisher>();
        services.AddSingleton<RabbitMqSettings>();
        services.AddSingleton<ISerializer, JsonSerializer>();
        services.AddSingleton<ILogger>(nlog);
    })
    .Build();

host.Run();
