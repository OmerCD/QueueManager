using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using QueueManager.Contract;
using QueueManager.Core;
using QueueManager.QueueManagement;
using QueueManager.RabbitMq.ConnectionManager;

namespace QueueManager.RabbitMq.DependencyInjection
{
    public class RabbitMqPublisherBuilder<TPublisher> where TPublisher : class, IPublisher
    {
        private readonly IConfiguration _configuration;
        private readonly TPublisher _publisher;
        private readonly IDictionary<Type, IQueuePublisher> _queuePublishers;
        private IQueueConnectionFactory _connectionFactory;

        public RabbitMqPublisherBuilder(IConfiguration configuration, IPublisher publisher)
        {
            _configuration = configuration;
            _publisher = (TPublisher) publisher;
            _queuePublishers = _publisher.QueuePublishers;
        }

        internal RabbitMqPublisherBuilder<TPublisher> WithConnectionFactory(IQueueConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            return this;
        }

        public RabbitMqPublisherBuilder<TPublisher> AddQueuePublisher<TMessage, TQueuePublisher>(
            IQueueProperties queueProperties)
            where TMessage : class, IQueueMessage
            where TQueuePublisher : class, IQueuePublisher, new()
        {
            var queuePublisher = new TQueuePublisher();
            return AddQueuePublisher<TMessage, TQueuePublisher>(queueProperties, queuePublisher);
        }

        private RabbitMqPublisherBuilder<TPublisher> AddQueuePublisher<TMessage, TQueuePublisher>(IQueueProperties queueProperties,
            TQueuePublisher queuePublisher)
            where TMessage : class, IQueueMessage
            where TQueuePublisher : class, IQueuePublisher, new()
        {
            queuePublisher.SetQueueProperties(queueProperties);
            var publisherQueuePublishers = _queuePublishers;
            publisherQueuePublishers.TryAdd(typeof(TMessage), queuePublisher);
            return this;
        }

        public RabbitMqPublisherBuilder<TPublisher> AddQueuePublisher<TMessage, TQueuePublisher>(
            string queueConfigurationName)
            where TMessage : class, IQueueMessage
            where TQueuePublisher : class, IQueuePublisher, new()
        {
            var queueProperties = _configuration.GetSection($"RabbitMqQueues:{queueConfigurationName}")
                .Get<QueueProperties>();
            return AddQueuePublisher<TMessage, TQueuePublisher>(queueProperties);
        }

        public RabbitMqPublisherBuilder<TPublisher> AddQueuePublisher<TMessage, TQueuePublisher>()
            where TMessage : class, IQueueMessage
            where TQueuePublisher : class, IQueuePublisher, new()
        {
            var queuePublisher = new TQueuePublisher();
            var queueProperties = _configuration.GetSection($"RabbitMqQueues:{queuePublisher.QueueSettingsName}")
                .Get<QueueProperties>();
            return AddQueuePublisher<TMessage, TQueuePublisher>(queueProperties, queuePublisher);
        }

        internal TPublisher Build()
        {
            if (_connectionFactory != null)
            {
                _publisher.SetConnectionFactory(_connectionFactory);
            }

            return _publisher;
        }
    }
}