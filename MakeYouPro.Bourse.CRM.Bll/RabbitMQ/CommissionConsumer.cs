using MakeYouPro.Bourse.CRM.Core.RabbitMQ;
using MakeYouPro.Bourse.CRM.Core.RabbitMQ.Models;
using NLog;
using RabbitMQ.Client;

namespace MakeYouPro.Bourse.CRM.Bll.RabbitMQ
{
    public class CommissionConsumer : ConsumerBase<CommissionMessage>
    {
        public CommissionConsumer(IConnection connection, string queueName, ILogger nLogger) : base(connection, queueName, nLogger)
        {
        }

        protected override void HandleMessage(CommissionMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
