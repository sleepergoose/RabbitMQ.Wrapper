using RabbitMQ.Client;
using RabbitMQ.Wrapper.Models;
using RabbitMQ.Wrapper.Interfaces;

namespace RabbitMQ.Wrapper.Services
{
    public class ProducerScopeFactory : IProducerScopeFactory
    {
        private readonly IConnectionFactory _connectionFactory;

        public ProducerScopeFactory(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IProducerScope Open(ScopeSettings messageScopeSettings)
        {
            return new ProducerScope(_connectionFactory, messageScopeSettings);
        }
    }
}
