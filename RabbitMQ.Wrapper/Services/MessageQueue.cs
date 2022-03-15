using RabbitMQ.Client;
using RabbitMQ.Wrapper.Interfaces;
using RabbitMQ.Wrapper.Models;

namespace RabbitMQ.Wrapper.Services
{
    public class MessageQueue : IMessageQueue
    {
        private readonly IConnection _connection;

        public IModel Channel { get; set; }

        public MessageQueue(IConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection();
            Channel = _connection.CreateModel();
        }

        public MessageQueue(IConnectionFactory connectionFactory, ScopeSettings scopeSettings)
            :this(connectionFactory)
        {
            DeclareExchange(scopeSettings.ExchangeName, scopeSettings.ExchangeType);

            if (scopeSettings.QueueName != null)
            {
                BindQueue(scopeSettings.QueueName, scopeSettings.ExchangeName, scopeSettings.RoutingKey);
            }
        }

        public void BindQueue(string queueName, string exchangeName, string routingKey)
        {
            Channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            Channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routingKey);
        }

        public void DeclareExchange(string exchangeName, string exchangeType)
        {
            Channel.ExchangeDeclare(exchangeName, exchangeType ?? string.Empty);
        }

        public void Dispose()
        {
            Channel?.Close();
            _connection?.Close();
        }
    }
}
