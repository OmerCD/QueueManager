using MediatR;

namespace QueueManager.Contract
{
    public interface IQueueMessage : IRequest<DeliveryEvents>
    {
    }
}