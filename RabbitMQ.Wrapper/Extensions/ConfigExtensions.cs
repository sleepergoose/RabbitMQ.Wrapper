using System;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Wrapper.Interfaces;
using RabbitMQ.Wrapper.Services;

namespace RabbitMQ.Wrapper.Extensions
{
    public static class ConfigExtensions
    {
        public static void AddRabbitMQService(this IServiceCollection services, string uri)
        {
            services.AddScoped<IProducer, Producer>();
            services.AddScoped<IConsumer, Consumer>();
            services.AddScoped<IProducerScope, ProducerScope>();
            services.AddScoped<IConsumerScope, ConsumerScope>();
            services.AddScoped<IProducerScopeFactory, ProducerScopeFactory>();
            services.AddScoped<IConsumerScopeFactory, ConsumerScopeFactory>();
            services.AddScoped<IMessageQueue, MessageQueue>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IConnectionFactory>(provider =>
                new RabbitConnectionFactory(new Uri(uri)).ConnectionFactory);
        }
    }
}
