namespace QueueManager.Contract
{
    public class DeliveryEvents
    {
        public bool Acknowledge { get; set; }
        public bool Requeue { get; set; }
    }
}