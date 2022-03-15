using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Wrapper.Interfaces;
using RabbitMQ.Wrapper.Models;
using RabbitMQ.Wrapper.Services;
using System;
using System.Text;
using System.Text.Json;

namespace RabbitMQ.Listener
{
    internal class Program
    {
        private static IConfiguration Configuration { get; set; }
        private static IServiceProvider _services;

        static void Main(string[] args)
        {
            // configures the app
            AppConfigure();

            // RabbitMQ settings
            ScopeSettings producerSettings = new ScopeSettings
            {
                ExchangeName = Configuration["ProducerSettings:ExchangeName"],
                ExchangeType = Configuration["ProducerSettings:ExchangeType"],
                QueueName = Configuration["ProducerSettings:QueueName"],
                RoutingKey = Configuration["ProducerSettings:RoutingKey"]
            };

            ScopeSettings consumerSettings = new ScopeSettings {
                ExchangeName = Configuration["ConsumerSettings:ExchangeName"],
                ExchangeType = Configuration["ConsumerSettings:ExchangeType"],
                QueueName = Configuration["ConsumerSettings:QueueName"],
                RoutingKey = Configuration["ConsumerSettings:RoutingKey"]
            };

            // Get the main message service with IoC
            IMessageService messageService = _services.GetService<IMessageService>();

            messageService.SetMessageService(producerSettings, consumerSettings);
            messageService.MessageReceived += MessageService_MessageReceived;

            Console.ReadLine();

            messageService.Dispose();
        }

        private static void MessageService_MessageReceived(string message)
        {
            // var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(message);
        }

        static void AppConfigure()
        {
            // app settings
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            Configuration = builder.Build();

            var uri = Configuration.GetSection("RabbitMQUri").Value;

            // DI
            _services = new ServiceCollection()
                .AddScoped<IProducer, Producer>()
                .AddScoped<IConsumer, Consumer>()
                .AddScoped<IProducerScope, ProducerScope>()
                .AddScoped<IConsumerScope, ConsumerScope>()
                .AddScoped<IProducerScopeFactory, ProducerScopeFactory>()
                .AddScoped<IConsumerScopeFactory, ConsumerScopeFactory>()
                .AddScoped<IMessageQueue, MessageQueue>()
                .AddScoped<IMessageService, MessageService>()
                .AddScoped<RabbitConnectionFactory>()
                .AddScoped<IConnectionFactory>(provider => 
                    new RabbitConnectionFactory(new Uri(uri)).ConnectionFactory)
                .BuildServiceProvider();
        }
    }
}
