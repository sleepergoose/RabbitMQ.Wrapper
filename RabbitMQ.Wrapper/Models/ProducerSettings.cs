using RabbitMQ.Client;
using RabbitMQ.Wrapper.Interfaces;

namespace RabbitMQ.Wrapper.Models
{
    internal class ProducerSettings
    {
        public IModel Channel { get; set; }
        public PublicationAddress PublicationAddress { get; set; }
    }
}
