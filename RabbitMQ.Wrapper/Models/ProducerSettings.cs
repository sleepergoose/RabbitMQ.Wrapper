using RabbitMQ.Client;
using RabbitMQ.Wrapper.Interfaces;

namespace RabbitMQ.Wrapper.Models
{
    public class ProducerSettings
    {
        public IModel Channel { get; set; }
        public PublicationAddress PublicationAddress { get; set; }
    }
}
