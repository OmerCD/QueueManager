using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QueueManager.Contract;
using TestContract;
using QueueManager.QueueManagement;

namespace TestAppConsumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IQueueConsumer<TestPublishEmailModel> _queueConsumer;
        private readonly IQueueConsumer<TestPublishSmsModel> _queueSmsConsumer;

        public Worker(ILogger<Worker> logger
            // , IQueueConsumer<TestPublishEmailModel> queueConsumer, IQueueConsumer<TestPublishSmsModel> queueSmsConsumer
            )
        {
            _logger = logger;
            // _queueConsumer = queueConsumer;
            // _queueSmsConsumer = queueSmsConsumer;
            // _queueSmsConsumer.OnConsumed += QueueSmsConsumerOnOnConsumed;
            // _queueConsumer.OnConsumed+=QueueConsumerOnConsumed;
            // _queueConsumer.StartConsuming();
            // _queueSmsConsumer.StartConsuming();
        }

        private Task QueueSmsConsumerOnOnConsumed(TestPublishSmsModel arg1, DeliveryEvents arg2)
        {
            arg2.Acknowledge = true;
            _logger.LogInformation($"Consumed : {JsonSerializer.Serialize(arg1)}");
            return Task.CompletedTask;        }

        private Task QueueConsumerOnConsumed(TestPublishEmailModel arg1, DeliveryEvents arg2)
        {
            arg2.Acknowledge = true;
            _logger.LogInformation($"Consumed : {JsonSerializer.Serialize(arg1)}");
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}