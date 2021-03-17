namespace QueueManager.RabbitMq.Publisher
{
    public class SmsPublisher : BaseRabbitMqQueuePublisher
    {
        public override string QueueSettingsName => "Sms";
    }
}