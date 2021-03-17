using System;
using System.Threading;
using System.Threading.Tasks;
using QueueManager.Contract;
using TestContract;
using QueueManager.RabbitMq.Consumer;

namespace TestAppConsumer
{
    public class TestEmailHandler : IBaseRequestHandler<EmailModel>
    {
        public Task<DeliveryEvents> Handle(EmailModel request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Message Received");
            return Task.FromResult(new DeliveryEvents()
            {
                Acknowledge = true
            });
        }
    }
}