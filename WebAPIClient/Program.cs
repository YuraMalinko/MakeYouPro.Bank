using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using WebAPIClient;

public class Program
{
    Dictionary<string, decimal> ratesForRabbit = RateStorage.rateDictionary;
    static void Main(string[] args)

    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
        builder.Services.AddHostedService<TimedBackgroundService>();
        IHost host = builder.Build();
        host.Run();

        var factory = new ConnectionFactory { HostName = "main" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();


        channel.QueueDeclare(queue: "MakeYouPro",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);
        string mail = "test";
        var body = Encoding.UTF8.GetBytes(mail);

        channel.BasicPublish(exchange: string.Empty,
                             routingKey: "ratesProvider",
                             basicProperties: null,
                             body: body);
        Console.WriteLine($" [x] Sent ");

        Console.ReadLine();
    }
}
