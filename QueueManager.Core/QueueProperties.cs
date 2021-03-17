namespace QueueManager.Core
{
    public class QueueProperties : IQueueProperties
    {
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public string RouteKey { get; set; }
    }
}