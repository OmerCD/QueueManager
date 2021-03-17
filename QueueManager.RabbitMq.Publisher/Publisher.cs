using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using QueueManager.Contract;
using QueueManager.QueueManagement;
using QueueManager.RabbitMq.ConnectionManager;
using QueueManager.RabbitMq.Publisher.Exceptions;
using RabbitMQ.Client;

namespace QueueManager.RabbitMq.Publisher
{
    public class DirectPublisher : IPublisher
    {
        private readonly ConcurrentDictionary<Type, IQueuePublisher> _queuePublishers;
        private IModel _model;
        private IBasicProperties _properties;
        private readonly ILogger<DirectPublisher> _logger;

        public DirectPublisher(ILogger<DirectPublisher> logger)
        {
            _logger = logger;
            _queuePublishers = new ConcurrentDictionary<Type, IQueuePublisher>();
        }

        public void DeclareQueuesAndExchanges()
        {
            foreach (var (_, queuePublisher) in _queuePublishers)
            {
                queuePublisher.DeclareQueueExchange(_model);
            }
        }
        public void Publish<TModel>(TModel model) where TModel : IQueueMessage
        {
            var type = typeof(TModel);
            try
            {
                if (_queuePublishers.TryGetValue(type, out var publishProfile))
                {
                    _model.BasicPublish(publishProfile.QueueProperties.ExchangeName,
                        publishProfile.QueueProperties.RouteKey, _properties, model.Serialize());
                    _logger?.LogInformation(
                        $"Published {JsonSerializer.Serialize(model)} to {publishProfile.QueueProperties.ExchangeName} with key {publishProfile.QueueProperties.RouteKey}");
                }
                else
                {
                    throw new PublisherNotFoundException(type);
                }
            }
            catch (Exception e)
            {
                _logger?.LogError($"Error while publishing {JsonSerializer.Serialize(model)}", e);
                throw;
            }
        }

        public IDictionary<Type, IQueuePublisher> QueuePublishers => _queuePublishers;
        public void SetConnectionFactory(IQueueConnectionFactory connectionFactory)
        {
            _model = connectionFactory.CreateConnection().CreateModel();
            _properties = _model.CreateBasicProperties();
        }
    }
}