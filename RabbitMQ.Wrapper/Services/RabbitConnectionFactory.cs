using RabbitMQ.Client;
using System;

namespace RabbitMQ.Wrapper.Services
{
    public class RabbitConnectionFactory
    {
        public ConnectionFactory ConnectionFactory { get; set; }

        public RabbitConnectionFactory(Uri uri)
        {
            ConnectionFactory = new ConnectionFactory
            {
                Uri = uri,
                AutomaticRecoveryEnabled = true,
                TopologyRecoveryEnabled = true, 
                RequestedConnectionTimeout = TimeSpan.FromSeconds(15),
                SocketReadTimeout = TimeSpan.FromSeconds(10),
                SocketWriteTimeout = TimeSpan.FromSeconds(10),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(30)
            };
        }
    }
}
