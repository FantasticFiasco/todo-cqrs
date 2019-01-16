using EasyNetQ;
using Messaging.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadModel;
using ReadModel.NoSql;

namespace Reads.Synchronizer
{
    public static class Extensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection self)
        {
            Schema.Create();

            return self;
        }

        public static IServiceCollection AddMessaging(this IServiceCollection self, IConfiguration configuration)
        {
            return self
                .AddSingleton(_ => BuildBus(configuration))
                .AddSingleton<RabbitMQEventConsumer>()
                .AddSingleton(_ => BuildConnectionString(configuration))
                .AddSingleton<ITodoListSynchronizer, NoSqlTodoList>();
        }

        private static IBus BuildBus(IConfiguration configuration)
        {
            var host = configuration["MESSAGING_HOST"];
            var username = configuration["MESSAGING_USER"];
            var password = configuration["MESSAGING_PASSWORD"];
            var connectionString = $"host={host};username={username};password={password}";

            return RabbitHutch.CreateBus(connectionString);
        }

        private static ConnectionString BuildConnectionString(IConfiguration configuration)
        {
            var host = configuration["READ_MODEL_HOST"];
            var username = configuration["READ_MODEL_USER"];
            var password = configuration["READ_MODEL_PASSWORD"];

            return new ConnectionString(host, username, password);
        }
    }
}
