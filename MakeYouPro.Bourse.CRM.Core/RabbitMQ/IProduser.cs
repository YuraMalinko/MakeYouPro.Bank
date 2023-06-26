namespace MakeYouPro.Bourse.CRM.Core.RabbitMQ
{
    public interface IProduser<T>
    {
        void Publish(T value);
    }
}