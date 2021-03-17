using System.Collections.Generic;

namespace QueueManager.Core
{
    public class ConsumerQueuesList
    {
        public Dictionary<string, string> ConsumerQueueList { get; set; }

        public string this[string consumerHandlerType] => ConsumerQueueList[consumerHandlerType];
    }
}