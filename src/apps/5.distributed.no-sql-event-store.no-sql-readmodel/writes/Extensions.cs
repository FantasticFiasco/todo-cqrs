using Cqrs;
using EasyNetQ;
using EventStore.NoSql;
using Messaging.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Todo;

namespace Writes
{
    public static class Extensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection self)
        {
            Schema.Create();

            return self;
        }

        public static IServiceCollection AddCqrs(this IServiceCollection self, IConfiguration configuration)
        {
            // Write model
            self
                .AddSingleton(_ => BuildConnectionString(configuration))
                .AddSingleton<IEventStore, NoSqlEventStore>();

            // Command relay
            self.AddSingleton<ICommandRelay>(provider =>
            {
                // Create the command relay
                var eventStore = provider.GetRequiredService<IEventStore>();
                var commandRelay = new CommandRelay(eventStore);

                // Let the command relay scan the aggregate and register its command handlers
                commandRelay.RegisterHandlersFor<TodoAggregate>();

                // Let the command relay scan the event publisher and register its publishers
                var eventProcessor = provider.GetRequiredService<RabbitMQEventPublisher>();
                commandRelay.RegisterPublishersFor(eventProcessor);

                return commandRelay;
            });

            // Messaging
            self
                .AddSingleton(_ => BuildBus(configuration))
                .AddSingleton<RabbitMQEventPublisher>();

            return self;
        }

        private static ConnectionString BuildConnectionString(IConfiguration configuration)
        {
            var host = configuration["EVENT_STORE_HOST"];
            var username = configuration["EVENT_STORE_USER"];
            var password = configuration["EVENT_STORE_PASSWORD"];

            return new ConnectionString(host, username, password);
        }

        private static IBus BuildBus(IConfiguration configuration)
        {
            var host = configuration["MESSAGING_HOST"];
            var username = configuration["MESSAGING_USER"];
            var password = configuration["MESSAGING_PASSWORD"];
            var connectionString = $"host={host};username={username};password={password}";

            return RabbitHutch.CreateBus(connectionString);
        }
    }
}
