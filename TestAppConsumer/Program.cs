using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QueueManager.RabbitMq.DependencyInjection;

namespace TestAppConsumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddQueueConnection(hostContext.Configuration);
                    services.AddQueueHandling(typeof(TestEmailHandler), typeof(AsyncEmailConsumer),
                        hostContext.Configuration);
                    services.AddHostedService<Worker>();
                });
    }
}