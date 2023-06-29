using MakeYouPro.Bourse.LeadStatusUpdater.Service;
using NLog;
using ILogger = NLog.ILogger;
using LogManager = NLog.LogManager;

LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
var nlog = LogManager.Setup().GetCurrentClassLogger();

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<ILogger>(nlog);
    })
    .Build();

host.Run();
