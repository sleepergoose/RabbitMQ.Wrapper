namespace RabbitMQ.Wrapper.Interfaces
{
    public interface IProducer
    {
        void Send(string message, string type = null);
        void Send(byte[] body, string type = null);
    }
}
