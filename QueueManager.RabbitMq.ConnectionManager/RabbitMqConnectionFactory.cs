using QueueManager.Core;
using RabbitMQ.Client;

namespace QueueManager.RabbitMq.ConnectionManager
{
    public class RabbitMqConnectionFactory : IQueueConnectionFactory
    {
        private readonly ConnectionProperties _connectionProperties;

        public RabbitMqConnectionFactory(ConnectionProperties connectionProperties)
        {
            _connectionProperties = connectionProperties;
        }
        public IConnection CreateConnection(bool isAsyncConsumer = false)
        {
            
            var factory = new ConnectionFactory
            {
                HostName = _connectionProperties.HostName,
                Password = _connectionProperties.Password,
                UserName = _connectionProperties.UserName
            };
            if (isAsyncConsumer)
            {
                factory.DispatchConsumersAsync = true;
            }
            return factory.CreateConnection();
        }
    }
}