using QueueManager.Contract;

namespace TestContract
{
    public class TestPublishSmsModel : IQueueMessage
    {
        public string Content { get; set; }
    }
}