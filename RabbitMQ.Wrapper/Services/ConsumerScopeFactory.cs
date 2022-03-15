using RabbitMQ.Client;
using RabbitMQ.Wrapper.Models;
using RabbitMQ.Wrapper.Interfaces;

namespace RabbitMQ.Wrapper.Services
{
    internal class ConsumerScopeFactory : IConsumerScopeFactory
    {
        private readonly IConnectionFactory _connectionFactory;

        public ConsumerScopeFactory(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IConsumerScope Open(ScopeSettings ScopeSettings)
        {
            return new ConsumerScope(_connectionFactory, ScopeSettings);
        }

        public IConsumerScope Connect(ScopeSettings ScopeSettings)
        {
            var ConsumerScope = Open(ScopeSettings);
            ConsumerScope.Consumer.Connect();

            return ConsumerScope;
        }
    }
}
