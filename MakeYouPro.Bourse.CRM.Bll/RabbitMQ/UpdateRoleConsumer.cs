using MakeYouPro.Bourse.CRM.Bll.RabbitMQ.Models;
using MakeYouPro.Bourse.CRM.Core.RabbitMQ;
using MakeYouPro.Bourse.CRM.Dal.IRepositories;
using NLog;
using RabbitMQ.Client;

namespace MakeYouPro.Bourse.CRM.Bll.RabbitMQ
{
    public class UpdateRoleConsumer : ConsumerBase<UpdateRoleMessage>
    {
        private readonly ILeadRepository _leadSRepository;

        public UpdateRoleConsumer(IConnection connection, string queueName,
                                 ILeadRepository leadSRepository, ILogger nLogger) : base(connection, queueName, nLogger)
        {
            _leadSRepository = leadSRepository;
        }

        protected override void HandleMessage(UpdateRoleMessage message)
        {
            _leadSRepository.UpdateLeadRoleAsync(message.Status, message.Id);
        }
    }
}
