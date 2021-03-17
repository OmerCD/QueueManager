using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using QueueManager.Contract;
using QueueManager.Core;
using QueueManager.QueueManagement;
using QueueManager.RabbitMq.ConnectionManager;
using QueueManager.RabbitMq.Consumer.Exceptions;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace QueueManager.RabbitMq.Consumer
{
    public abstract class BaseAsyncRabbitMqConsumer<TModel, TSelf> : IQueueConsumer<TModel>
        where TModel : IQueueMessage
        where TSelf : BaseAsyncRabbitMqConsumer<TModel, TSelf>
    {
        protected readonly IModel Model;
        protected readonly AsyncEventingBasicConsumer Consumer;
        protected readonly ILogger<TSelf> Logger;
        protected readonly IMediator Mediator;

        protected BaseAsyncRabbitMqConsumer(IQueueConnectionFactory connectionFactory, ILogger<TSelf> logger,
            ConsumerQueuesList consumerQueuesList, IMediator mediator)
        {
            Logger = logger;
            Mediator = mediator;
            Model = connectionFactory.CreateConnection(true).CreateModel();
            Consumer = new AsyncEventingBasicConsumer(Model);
            Consumer.Received += ConsumerOnReceived;
            // ReSharper disable once VirtualMemberCallInConstructor
            QueueName = consumerQueuesList[ConsumerHandlerType];
        }

        public void StartConsuming()
        {
            if (string.IsNullOrWhiteSpace(QueueName))
            {
                throw new QueueNameEmptyException($"For {GetType().Name} Queue Name is not assigned or empty");
            }

            Model.BasicConsume(QueueName, false, Consumer);
            Logger?.LogInformation($"Consume started for {GetType().Name}");
        }

        protected async Task ConsumerOnReceived(object sender, BasicDeliverEventArgs @event)
        {
            var deliveryEvents = new DeliveryEvents();
            var body = @event.Body.ToArray().Deserialize<TModel>();
            if (OnConsumed != null)
            {
                await OnConsumed(body, deliveryEvents);
                if (deliveryEvents.Acknowledge)
                {
                    Model.BasicAck(@event.DeliveryTag, false);
                }
                else
                {
                    Model.BasicNack(@event.DeliveryTag, false, deliveryEvents.Requeue);
                }
            }
            deliveryEvents = await Mediator.Send(body);
            if (deliveryEvents.Acknowledge)
            {
                Model.BasicAck(@event.DeliveryTag, false);
            }
            else
            {
                Model.BasicNack(@event.DeliveryTag, false, deliveryEvents.Requeue);
            }
        }

        public string QueueName { get; protected set; }

        public abstract string ConsumerHandlerType { get; }
        public event Func<TModel, DeliveryEvents, Task> OnConsumed;
    
    }
}