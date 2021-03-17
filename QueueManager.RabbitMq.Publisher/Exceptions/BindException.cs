using System;

namespace QueueManager.RabbitMq.Publisher.Exceptions
{

    [Serializable]
    public class BindException : Exception
    {
        public BindException() { }
        public BindException(string message) : base(message) { }
        public BindException(string message, Exception inner) : base(message, inner) { }
        protected BindException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
