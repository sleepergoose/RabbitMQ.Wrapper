using RabbitMQ.Wrapper.Models;

namespace RabbitMQ.Wrapper.Interfaces
{
    public interface IProducerScopeFactory
    {
        IProducerScope Open(ScopeSettings scopeSettings);
    }
}
