using RabbitMQ.Client;
using QueueManager.Core;

namespace QueueManager.QueueManagement
{
    public interface IQueuePublisher
    {
        IQueueProperties QueueProperties { get; }
        void SetQueueProperties(IQueueProperties queueProperties);
        string QueueSettingsName { get; }

        /// <summary>
        /// Declares queue and/or exchange if not declared before. If both of them declared, binds them.
        /// </summary>
        QueueDeclareOk DeclareQueueExchange(IModel model, string exchangeType = ExchangeType.Direct);
    }
}