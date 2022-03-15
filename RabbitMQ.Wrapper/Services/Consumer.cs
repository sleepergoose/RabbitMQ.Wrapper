using RabbitMQ.Client.Events;
using RabbitMQ.Wrapper.Interfaces;
using RabbitMQ.Wrapper.Models;
using System;
using System.Collections.Generic;

namespace RabbitMQ.Wrapper.Services
{
    public class Consumer : IConsumer
    {
        private readonly ConsumerSettings _consumerSettings;
        private readonly EventingBasicConsumer _consumer;

        public Consumer(ConsumerSettings consumerSettings)
        {
            _consumerSettings = consumerSettings;
            _consumer = new EventingBasicConsumer(_consumerSettings.Channel);
        }

        public event EventHandler<BasicDeliverEventArgs> Received
        {
            add => _consumer.Received += value;
            remove => _consumer.Received -= value;
        }

        public void Connect()
        {
            if (_consumerSettings.SequentialFetch == true)
            {
                // BasicQos method with the prefetchCount = 1 setting tells RabbitMQ not to give more than one message to a worker at a time
                // in other words, don't dispatch a new message to a worker until it has processed and acknowledged the previous one.
                // Instead, it will dispatch it to the next worker that is not still busy (https://www.rabbitmq.com/tutorials/tutorial-two-dotnet.html)
                _consumerSettings.Channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
            }

            _consumerSettings.Channel.BasicConsume(
                queue: _consumerSettings.QueueName,
                autoAck: _consumerSettings.AutoAcknowledge, 
                consumerTag: "",
                noLocal: true,
                exclusive: false, 
                arguments: new Dictionary<string, object>() { },
                consumer: _consumer);
        }

        public void SetAcknowledge(ulong deliveryTag, bool processed)
        {
            if (processed == true)
            {
                _consumerSettings.Channel.BasicAck(deliveryTag: deliveryTag, multiple: false);
            }
            else
            {
                _consumerSettings.Channel.BasicNack(deliveryTag: deliveryTag, multiple: false, requeue: true);
            }
        }
    }
}
