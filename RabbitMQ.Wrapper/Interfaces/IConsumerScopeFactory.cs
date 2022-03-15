using RabbitMQ.Wrapper.Models;

namespace RabbitMQ.Wrapper.Interfaces
{
    public interface IConsumerScopeFactory
    {
        IConsumerScope Open(ScopeSettings scopeSettings);

        IConsumerScope Connect(ScopeSettings scopeSettings);
    }
}
