using RabbitMQ.Client;

namespace RabbitMQ.Wrapper.Interfaces
{
    public interface IMessageQueue
    {
        IModel Channel { get; set; }

        void DeclareExchange(string exchangeName, string exchangeType);

        void BindQueue(string queueName, string exchangeName, string routingKey);

        void Dispose();
    }
}
