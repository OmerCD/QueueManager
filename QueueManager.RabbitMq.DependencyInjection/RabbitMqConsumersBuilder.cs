using System;
using System.Collections.Generic;
using QueueManager.Contract;
using QueueManager.QueueManagement;

namespace QueueManager.RabbitMq.DependencyInjection
{
    public class RabbitMqConsumersBuilder
    {
        public List<(Type handlerType, Type modelType)> Consumers { get; } = new();

        public void AddConsumer<TQueueConsumer, TModel>() 
            where TModel : IQueueMessage
            where TQueueConsumer : class, IQueueConsumer<TModel>
        {
            Consumers.Add((typeof(TQueueConsumer), typeof(TModel)));
        }

        internal IEnumerable<(Type handlerType, Type modelType)> Build()
        {
            return Consumers;
        }
    }
}