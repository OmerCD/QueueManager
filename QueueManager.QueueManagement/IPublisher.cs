using System;
using System.Collections.Generic;
using QueueManager.Contract;
using QueueManager.RabbitMq.ConnectionManager;

namespace QueueManager.QueueManagement
{
    public interface IPublisher
    {
        void Publish<TModel>(TModel model) where TModel : IQueueMessage;
        IDictionary<Type, IQueuePublisher> QueuePublishers { get; }
        void SetConnectionFactory(IQueueConnectionFactory connectionFactory);
        void DeclareQueuesAndExchanges();
    }
}