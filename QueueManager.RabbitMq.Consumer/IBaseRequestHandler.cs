using MediatR;
using QueueManager.Contract;

namespace QueueManager.RabbitMq.Consumer
{
    public interface IBaseRequestHandler<in T> : IRequestHandler<T, DeliveryEvents> where T : IRequest<DeliveryEvents>
    {
        
    }
}