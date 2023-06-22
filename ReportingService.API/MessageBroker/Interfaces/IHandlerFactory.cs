﻿using ReportingService.Api.MessageHandler;

namespace ReportingService.Api.MessageBroker.Interfaces
{
    public interface IHandlerFactory
    {
        IMessageHandler GetMessageHandler(string routingKey);

        void AddHandler(string routingKey, IMessageHandler handler);
    }
}