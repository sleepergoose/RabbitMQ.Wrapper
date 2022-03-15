using System;
using RabbitMQ.Client;
using RabbitMQ.Wrapper.Models;
using RabbitMQ.Wrapper.Interfaces;
using RabbitMQ.Wrapper.Services;

namespace RabbitMQ.Wrapper.Services
{
    public class ConsumerScope : IConsumerScope
    {
        // Fields
        private readonly ScopeSettings _messageScopeSettings;
        private readonly Lazy<IMessageQueue> _messageQueueLazy;
        private readonly Lazy<IConsumer> _messageConsumerLazy;
        private readonly IConnectionFactory _connectionFactory;

        // Properties
        public IConsumer Consumer => _messageConsumerLazy.Value;
        private IMessageQueue Queue => _messageQueueLazy.Value;

        // ctor
        public ConsumerScope(IConnectionFactory connectionFactory, ScopeSettings messageScopeSettings)
        {
            _connectionFactory = connectionFactory;
            _messageScopeSettings = messageScopeSettings;

            _messageQueueLazy = new Lazy<IMessageQueue>(CreateQueue);
            _messageConsumerLazy = new Lazy<IConsumer>(CreateConsumer);
        }


        // Methods
        private IMessageQueue CreateQueue()
        {
            return new MessageQueue(_connectionFactory, _messageScopeSettings);
        }

        private IConsumer CreateConsumer()
        {
            return new Consumer(new ConsumerSettings
            {
                Channel = Queue.Channel,
                QueueName = _messageScopeSettings.QueueName,

                // don't dispatch a new message to a worker until it has processed and acknowledged the previous one
                SequentialFetch = true 
            });
        }

        public void Dispose()
        {
            Queue?.Dispose();
        }
    }
}
