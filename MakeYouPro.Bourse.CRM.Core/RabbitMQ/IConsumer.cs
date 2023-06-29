using Microsoft.Extensions.Hosting;

namespace MakeYouPro.Bourse.CRM.Core.RabbitMQ
{
    public interface IConsumer<T> : IDisposable
    {
    }
}