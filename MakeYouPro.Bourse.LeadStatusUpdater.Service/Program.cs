using MakeYouPro.Bourse.LeadStatusUpdater.Service;
using MakeYouPro.Bourse.LeadStatusUpdater.Service.RabbitMq;
using MakeYouPro.Bourse.LeadStatusUpdater.Service.RabbitMq.Configuration;
using MakeYouPro.Bourse.LeadStatusUpdater.Service.RabbitMq.Interfaces;
using MakeYouPro.Bourse.LeadStatusUpdater.Service.RabbitMq.Serializer;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<RabbitMqPublisher>();
        services.AddSingleton<RabbitMqSettings>();
        services.AddSingleton<ISerializer, JsonSerializer>();
    })
    .Build();

host.Run();
