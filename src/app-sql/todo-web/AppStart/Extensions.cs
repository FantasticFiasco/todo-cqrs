using Cqrs;
using EventStore.Sql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Todo.ReadModel;

// ReSharper disable once CheckNamespace
namespace Todo.Web
{
    public static class Extensions
    {
        public static void AddCqrs(this IServiceCollection self, IConfiguration configuration)
        {
            var connectionString = BuildConnectionString(configuration);
            var eventStore = new SqlEventStore(connectionString);
            var todoList = new TodoList();

            var messageDispatcher = new MessageDispatcher(eventStore);
            messageDispatcher.ScanInstance(todoList);
            messageDispatcher.ScanInstance(new TodoAggregate());

            self.AddSingleton<ITodoList>(_ => todoList);
            self.AddSingleton(_ => messageDispatcher);
        }

        public static void AddDatabase(this IServiceCollection _, IConfiguration configuration)
        {
            var connectionString = BuildConnectionString(configuration);
            var schema = new Schema(connectionString);
            schema.Create();
        }

        private static string BuildConnectionString(IConfiguration configuration)
        {
            var username = configuration["DB_USER"];
            var password = configuration["DB_PASSWORD"];

            return $"Host=sql;Username={username};Password={password}";
        }
    }
}
