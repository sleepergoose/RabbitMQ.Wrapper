using RabbitMQ.Client.Events;
using System;

namespace RabbitMQ.Wrapper.Interfaces
{
    public interface IConsumer
    {
        event EventHandler<BasicDeliverEventArgs> Received;
        void Connect();
        void SetAcknowledge(ulong deliveryTag, bool processed);
    }
}
