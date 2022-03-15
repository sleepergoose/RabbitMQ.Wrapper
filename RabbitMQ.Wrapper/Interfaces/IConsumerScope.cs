namespace RabbitMQ.Wrapper.Interfaces
{
    public interface IConsumerScope
    {
        IConsumer Consumer { get; }
        void Dispose();
    }
}
