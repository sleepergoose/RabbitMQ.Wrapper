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

        public delegate void MessageDelegate(string message);
        public event MessageDelegate MessageReceived;

        public MessageService(IProducerScopeFactory producerScopeFactory,
            IConsumerScopeFactory consumerScopeFactory)
        {
            _producerScopeFactory = producerScopeFactory;
            _consumerScopeFactory = consumerScopeFactory;
        }
        
        public void ListenQueue(object model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            MessageReceived?.Invoke(message);

            _consumerScope.Consumer.SetAcknowledge(deliveryTag: ea.DeliveryTag, processed: true);
        }

        public void SendStringMessageToQueue(string message)
        {
            try
            {
                _producerScope.Producer.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SendMessageToQueue(byte[] body)
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

        public void SetMessageService(ScopeSettings producerSettings, ScopeSettings consumerSettings)
        {
            _producerScope = _producerScopeFactory.Open(producerSettings);
            _consumerScope = _consumerScopeFactory.Connect(consumerSettings);
            _consumerScope.Consumer.Received += ListenQueue;
        }

        public void Dispose()
        {
            _consumerScope?.Dispose();
        }
    }
}
