using RabbitMQ.Client;

namespace QueueManager.RabbitMq.ConnectionManager
{
    public interface IQueueConnectionFactory
    {
        IConnection CreateConnection(bool isAsyncConsumer = false);

    }
}