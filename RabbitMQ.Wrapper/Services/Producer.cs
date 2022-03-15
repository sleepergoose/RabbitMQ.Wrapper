using RabbitMQ.Client;
using RabbitMQ.Wrapper.Interfaces;
using RabbitMQ.Wrapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Wrapper.Services
{
    /// <summary>
    /// Producer - describes an object for transmiting data
    /// </summary>
    public class Producer : IProducer
    {
        private readonly ProducerSettings _producerSettings;
        private readonly IBasicProperties _basicProperties;

        public Producer(ProducerSettings producerSettings)
        {
            _producerSettings = producerSettings;

            _basicProperties = _producerSettings.Channel.CreateBasicProperties();
            //  it tells RabbitMQ to save the message to disk
            _basicProperties.Persistent = true;
        }

        /// <summary>
        /// Sends data to the exchange
        /// </summary>
        /// <param name="message">Text message for transmitting</param>
        /// <param name="type">Type of transmitted data</param>
        public void Send(string message, string type = null)
        {
            if (string.IsNullOrEmpty(type) == false)
            {
                _basicProperties.Type = type;
            }

            var body = Encoding.UTF8.GetBytes(message);

            _producerSettings.Channel.BasicPublish(
                _producerSettings.PublicationAddress,
                _basicProperties,
                body);
        }

        /// <summary>
        /// Sends data to the exchange
        /// </summary>
        /// <param name="body">Data for sending</param>
        /// <param name="type">Type of transmitted data</param>
        public void Send(byte[] body, string type = null)
        {
            if (string.IsNullOrEmpty(type) == false)
            {
                _basicProperties.Type = type;
            }

            _producerSettings.Channel.BasicPublish(
                _producerSettings.PublicationAddress,
                _basicProperties,
                body);
        }
    }
}
