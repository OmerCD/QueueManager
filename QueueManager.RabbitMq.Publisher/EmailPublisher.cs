namespace QueueManager.RabbitMq.Publisher
{
    public class EmailPublisher : BaseRabbitMqQueuePublisher
    {
        public override string QueueSettingsName => "Email";
    }
}