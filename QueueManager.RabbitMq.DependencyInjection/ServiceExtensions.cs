using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QueueManager.Core;
using QueueManager.QueueManagement;
using QueueManager.RabbitMq.ConnectionManager;
using QueueManager.RabbitMq.Publisher;
using MediatR;
using IPublisher = QueueManager.QueueManagement.IPublisher;

namespace QueueManager.RabbitMq.DependencyInjection
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddQueueConnection(this IServiceCollection services,
            IConfiguration configuration, string configurationName = "RabbitMqConnection")
        {
            var properties = configuration.GetSection(configurationName).Get<ConnectionProperties>();
            services.AddSingleton(properties);
            services.AddTransient<IQueueConnectionFactory, RabbitMqConnectionFactory>();
            return services;
        }

        public static IServiceCollection AddQueueHandling(this IServiceCollection services, Type handlerAssembly,
            Type consumerAssembly, IConfiguration configuration)
        {
            return services.AddQueueHandlers(handlerAssembly).AddQueueConsumers(consumerAssembly, configuration);
        }
        public static IServiceCollection AddQueueHandlers(this IServiceCollection services, Type assemblyType)
        {
            services.AddMediatR(assemblyType);
            return services;
        }
        public static IServiceCollection AddQueueConsumers(this IServiceCollection services, Type assemblyType,
            IConfiguration configuration)
        {
            var assembly = Assembly.GetAssembly(assemblyType);
            var consumerInterfaceType = typeof(IQueueConsumer);
            var consumerGenericType = typeof(IQueueConsumer<>);
            var consumerTypes = assembly.GetTypes()
                .Where(x => !x.IsAbstract && !x.IsInterface && consumerInterfaceType.IsAssignableFrom(x));
            var consumerTypesArray = consumerTypes as Type[] ?? consumerTypes.ToArray();
            foreach (var consumerType in consumerTypesArray)
            {
                var messageType = consumerType.BaseType?.GetGenericArguments()[0];
                var consumerInterfaceWithModelType = consumerGenericType.MakeGenericType(messageType);
                services.AddSingleton(consumerInterfaceWithModelType, consumerType);
            }
            AddConsumerList(services, configuration);

            var provider = services.BuildServiceProvider();
            foreach (var consumerType in consumerTypesArray)
            {
                var messageType = consumerType.BaseType?.GetGenericArguments()[0];
                var consumerInterfaceWithModelType = consumerGenericType.MakeGenericType(messageType);
                var instance = (IQueueConsumer) provider.GetService(consumerInterfaceWithModelType);
                instance.StartConsuming();
                services.AddSingleton(consumerInterfaceWithModelType, instance);
            }
            return services;
        }

        public static IServiceCollection AddQueueConsumers(this IServiceCollection services,
            IConfiguration configuration, Action<RabbitMqConsumersBuilder> builderOptions)
        {
            var builder = new RabbitMqConsumersBuilder();
            builderOptions(builder);
            var list = builder.Build();
            var handlerInterfaceType = typeof(IQueueConsumer<>);
            foreach (var type in list)
            {
                var genericInterfaceType = handlerInterfaceType.MakeGenericType(type.modelType);
                services.AddSingleton(genericInterfaceType, type.handlerType);
            }

            AddConsumerList(services, configuration);
            return services;
        }

        private static void AddConsumerList(IServiceCollection services, IConfiguration configuration)
        {
            var consumerDictionary = configuration.GetSection("ConsumerQueueList").GetChildren()
                .ToDictionary(x => x.Key, section => section.Value);
            var consumerQueueList = new ConsumerQueuesList()
            {
                ConsumerQueueList = consumerDictionary
            };
            services.AddSingleton(consumerQueueList);
        }

        public static IServiceCollection AddDirectQueuePublisher(this IServiceCollection services,
            Action<RabbitMqPublisherBuilder<DirectPublisher>> builder)
        {
            services.AddScoped<RabbitMqPublisherBuilder<DirectPublisher>>();
            services.AddSingleton<IPublisher, DirectPublisher>();
            var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var queueConnectionBuilder = scope.ServiceProvider
                .GetRequiredService<RabbitMqPublisherBuilder<DirectPublisher>>()
                .WithConnectionFactory(scope.ServiceProvider.GetService<IQueueConnectionFactory>());
            builder(queueConnectionBuilder);
            var publisher = queueConnectionBuilder.Build();
            publisher.DeclareQueuesAndExchanges();
            services.AddSingleton<IPublisher>(publisher);

            return services;
        }
    }
}