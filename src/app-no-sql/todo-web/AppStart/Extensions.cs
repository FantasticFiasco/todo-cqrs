using System;
using Cqrs;
using EventStore.NoSql;
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
            var eventStore = new NoSqlEventStore(connectionString);
            var todoList = new TodoList();

            var messageDispatcher = new MessageDispatcher(eventStore);
            messageDispatcher.ScanInstance(todoList);
            messageDispatcher.ScanInstance(new TodoAggregate());

            self.AddSingleton<ITodoList>(_ => todoList);
            self.AddSingleton(_ => messageDispatcher);
        }

        public static void AddDatabase(this IServiceCollection _)
        {
            //var schema = new Schema(ConnectionString);
            //schema.Create();
            throw new NotImplementedException();
        }

        private static string BuildConnectionString(IConfiguration configuration)
        {
            var username = configuration["DB_USER"];
            var password = configuration["DB_PASSWORD"];

            return $"mongodb://{username}:{password}@localhost:27017";
        }
    }
}
