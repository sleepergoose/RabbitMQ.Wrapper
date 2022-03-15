using System;
using RabbitMQ.Client.Events;
using RabbitMQ.Wrapper.Models;
using RabbitMQ.Wrapper.Services;

namespace RabbitMQ.Wrapper.Interfaces
{
    public interface IMessageService
    {
        event MessageService.MessageDelegate MessageReceived;

        void SetMessageService(ScopeSettings producerSettings, ScopeSettings consumerSettings);

        void SendStringMessageToQueue(string message);

        void SendMessageToQueue(byte[] body);

        void ListenQueue(Object model, BasicDeliverEventArgs ea);

        void Dispose();
    }
}
