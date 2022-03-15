using System;
using RabbitMQ.Client;
using RabbitMQ.Wrapper.Models;
using RabbitMQ.Wrapper.Interfaces;

namespace RabbitMQ.Wrapper.Services
{
    internal class ProducerScope : IProducerScope
    {
        // Fields
        private readonly ScopeSettings _ScopeSettings;
        private readonly Lazy<IMessageQueue> _QueueLazy;
        private readonly Lazy<IProducer> _ProducerLazy;
        private readonly IConnectionFactory _connectionFactory;

        // Properties
        public IProducer Producer => _ProducerLazy.Value;
        private IMessageQueue Queue => _QueueLazy.Value;

        // ctor
        public ProducerScope(IConnectionFactory connectionFactory, ScopeSettings ScopeSettings)
        {
            _connectionFactory = connectionFactory;
            _ScopeSettings = ScopeSettings;

            _QueueLazy = new Lazy<IMessageQueue>(CreateQueue);
            _ProducerLazy = new Lazy<IProducer>(CreateProducer);
        }

        // Methods
        private IMessageQueue CreateQueue()
        {
            return new MessageQueue(_connectionFactory, _ScopeSettings);
        }

        private IProducer CreateProducer()
        {
            return new Producer(new ProducerSettings
            {
                Channel = Queue.Channel,
                PublicationAddress = new PublicationAddress(_ScopeSettings.ExchangeType,
                                                            _ScopeSettings.ExchangeName,
                                                            _ScopeSettings.RoutingKey)
            });
        }

        public void Dispose()
        {
            Queue?.Dispose();
        }
    }
}
