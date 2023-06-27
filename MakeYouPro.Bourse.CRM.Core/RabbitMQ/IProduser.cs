namespace MakeYouPro.Bourse.CRM.Core.RabbitMQ
{
    public interface IProduser<T> : IDisposable
    {
        void Publish(T value);
    }
}