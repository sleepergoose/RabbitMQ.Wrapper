namespace RabbitMQ.Wrapper.Models
{
    public  class ScopeSettings
    {
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }
        public string QueueName { get; set; }
        public string RoutingKey { get; set; }
    }
}
