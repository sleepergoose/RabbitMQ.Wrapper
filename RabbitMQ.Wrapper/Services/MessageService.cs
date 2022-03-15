using RabbitMQ.Client.Events;
using RabbitMQ.Wrapper.Interfaces;
using RabbitMQ.Wrapper.Models;
using System;
using System.Text;

namespace RabbitMQ.Wrapper.Services
{
    public class MessageService : IMessageService
    {
        private IProducerScope _producerScope;
        private IConsumerScope _consumerScope;

        private readonly IProducerScopeFactory _producerScopeFactory;
        private readonly IConsumerScopeFactory _consumerScopeFactory;

        public delegate void MessageDelegate(byte[] body);
        public event MessageDelegate MessageReceived;

        public MessageService(IProducerScopeFactory producerScopeFactory,
            IConsumerScopeFactory consumerScopeFactory)
        {
            _producerScopeFactory = producerScopeFactory;
            _consumerScopeFactory = consumerScopeFactory;
        }

        /// <summary>
        /// Sets main settings and starts RabbitMQ Service
        /// </summary>
        /// <param name="producerSettings">General producer settings</param>
        /// <param name="consumerSettings">General consumer settings</param>
        public void SetMessageService(ScopeSettings producerSettings, ScopeSettings consumerSettings)
        {
            _producerScope = _producerScopeFactory.Open(producerSettings);
            _consumerScope = _consumerScopeFactory.Connect(consumerSettings);
            _consumerScope.Consumer.Received += ListenQueue;
        }

        public void ListenQueue(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray(); 
            MessageReceived?.Invoke(body);
            _consumerScope.Consumer.SetAcknowledge(deliveryTag: ea.DeliveryTag, processed: true);
        }

        /// <summary>
        /// Sends data to the queue
        /// </summary>
        /// <param name="body">Data for transmitting to the queue</param>
        /// <exception cref="Exception">Expeption when something went wrong</exception>
        public void SendDataToQueue(byte[] body)
        {
            try
            {
                _producerScope.Producer.Send(body);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Dispose()
        {
            _consumerScope?.Dispose();
        }
    }
}
