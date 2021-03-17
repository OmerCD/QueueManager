using MediatR;
using Microsoft.Extensions.Logging;
using QueueManager.Core;
using TestContract;
using QueueManager.RabbitMq.ConnectionManager;
using QueueManager.RabbitMq.Consumer;

namespace TestAppConsumer
{
    public class AsyncSmsConsumer : BaseAsyncRabbitMqConsumer<TestPublishSmsModel, AsyncSmsConsumer>
    {
        public AsyncSmsConsumer(IQueueConnectionFactory connectionFactory, ConsumerQueuesList consumerQueueList,
            ILogger<AsyncSmsConsumer> logger, IMediator mediator) : base(
            connectionFactory, logger, consumerQueueList, mediator)
        {
        }
        
        public sealed override string ConsumerHandlerType => "Sms";
    }

    public class AsyncEmailConsumer : BaseAsyncRabbitMqConsumer<EmailModel, AsyncEmailConsumer>
    {
        public AsyncEmailConsumer(IQueueConnectionFactory connectionFactory, ConsumerQueuesList consumerQueueList,
            ILogger<AsyncEmailConsumer> logger, IMediator mediator) : base(connectionFactory, logger, consumerQueueList,
            mediator)
        {
        }

        public sealed override string ConsumerHandlerType => "Email";
    }
}