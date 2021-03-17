using System;

namespace QueueManager.RabbitMq.Publisher.Exceptions
{

    [Serializable]
    public class DeclareException : Exception
    {
        public DeclareException() { }
        public DeclareException(string message) : base(message) { }
        public DeclareException(string message, Exception inner) : base(message, inner) { }
        protected DeclareException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
