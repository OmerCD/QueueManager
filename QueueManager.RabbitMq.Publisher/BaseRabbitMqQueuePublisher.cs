using System;
using QueueManager.Core;
using QueueManager.QueueManagement;
using RabbitMQ.Client;

namespace QueueManager.RabbitMq.Publisher
{
    public abstract class BaseRabbitMqQueuePublisher : IQueuePublisher
    {
        public IQueueProperties QueueProperties { get; private set; }
        public void SetQueueProperties(IQueueProperties queueProperties)
        {
            QueueProperties = queueProperties;
        }

        public abstract string QueueSettingsName { get; }

        /// <summary>
        /// Declares queue and/or exchange if not declared before. If both of them declared, binds them.
        /// </summary>
        public QueueDeclareOk DeclareQueueExchange(IModel model, string exchangeType = ExchangeType.Direct)
        {
            bool isExchangeEmpty = string.IsNullOrWhiteSpace(QueueProperties.ExchangeName);
            bool isQueueEmpty = string.IsNullOrWhiteSpace(QueueProperties.QueueName);
            if (isExchangeEmpty == false)
            {
                try
                {
                    model.ExchangeDeclare(QueueProperties.ExchangeName, exchangeType, true);
                }
                catch (Exception ex)
                {
                    throw new Exceptions.DeclareException($"Couldn't declare exchange {QueueProperties.ExchangeName}", ex);
                }
            }

            if (isQueueEmpty == false)
            {
                QueueDeclareOk queueDeclare;
                try
                {
                    queueDeclare = model.QueueDeclare(QueueProperties.QueueName, true, false, false, null);
                }
                catch (Exception ex)
                {
                    throw new Exceptions.DeclareException($"Couldn't declare queue {QueueProperties.QueueName}", ex);
                }

                if (isExchangeEmpty == false)
                {
                    try
                    {
                        model.QueueBind(QueueProperties.QueueName, QueueProperties.ExchangeName, QueueProperties.RouteKey ?? "");
                    }
                    catch (Exception ex)
                    {
                        throw new Exceptions.BindException(
                            $"Couldn't bind queue {QueueProperties.QueueName} to exchange {QueueProperties.ExchangeName}", ex);
                    }
                }

                return queueDeclare;
            }
            else
            {
                return null;
            }
        }
    }
}