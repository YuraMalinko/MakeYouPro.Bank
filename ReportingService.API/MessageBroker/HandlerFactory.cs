using ReportingService.Api.FinalRabbitMQ.MessageHandler;
using ReportingService.Api.MessageBroker.Interfaces;

namespace ReportingService.Api.MessageBroker
{
    public class HandlerFactory : IHandlerFactory
    {
        private IDictionary<string, IMessageHandler> handlers = new Dictionary<string, IMessageHandler>();

        public IMessageHandler GetMessageHandler(string routingKey)
        {
            if (handlers.ContainsKey(routingKey))
            {
                return handlers[routingKey];
            }

            return null;
        }

        public void AddHandler(string routingKey, IMessageHandler handler)
        {
            handlers[routingKey] = handler;
        }
    }
}
