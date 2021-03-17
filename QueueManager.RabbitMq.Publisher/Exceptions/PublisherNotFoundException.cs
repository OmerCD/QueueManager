using System;

namespace QueueManager.RabbitMq.Publisher.Exceptions
{
    public class PublisherNotFoundException : Exception
    {
        public PublisherNotFoundException(Type publishType) : base(
            $"Publisher for Type:{publishType.FullName} not found. Please register a publisher.")
        {
        }
    }
}