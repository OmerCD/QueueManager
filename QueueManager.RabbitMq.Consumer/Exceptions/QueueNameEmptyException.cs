using System;

namespace QueueManager.RabbitMq.Consumer.Exceptions
{
    public class QueueNameEmptyException : Exception
    {
        public QueueNameEmptyException(string message) : base(message)
        {
            
        }
    }
}