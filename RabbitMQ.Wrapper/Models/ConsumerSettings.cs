using RabbitMQ.Client;

namespace RabbitMQ.Wrapper.Models
{
    public class ConsumerSettings
    {
        public bool SequentialFetch { get; set; }

        public bool AutoAcknowledge { get; set; }

        public IModel Channel { get; set; }

        public string QueueName { get; set; }
    }
}
