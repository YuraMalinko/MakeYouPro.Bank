using System.Reflection;
using System.Text;
using System.Threading.Channels;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ;
using RabbitMQ.Client;
using WebAPIClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
