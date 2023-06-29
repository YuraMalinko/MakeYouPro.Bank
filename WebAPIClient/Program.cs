using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using WebAPIClient;

public class Program
{
  
    static void Main(string[] args)

    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<TimedBackgroundService>();
        IHost host = builder.Build();
        host.Run();


        Console.ReadLine();
    }
}
