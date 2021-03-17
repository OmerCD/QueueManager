namespace QueueManager.Core
{
    public interface IQueueProperties
    {
        public string QueueName { get; }
        public string ExchangeName { get; }
        public string RouteKey { get; }
    }
}