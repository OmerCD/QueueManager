using System;
using System.Threading.Tasks;
using QueueManager.Contract;

namespace QueueManager.QueueManagement
{
    public interface IQueueConsumer
    {
        void StartConsuming();
    }
    public interface IQueueConsumer<out TModel> : IQueueConsumer where TModel : IQueueMessage
    {
        string QueueName { get; }
        string ConsumerHandlerType { get; }
        event Func<TModel,DeliveryEvents, Task> OnConsumed;
    }
}