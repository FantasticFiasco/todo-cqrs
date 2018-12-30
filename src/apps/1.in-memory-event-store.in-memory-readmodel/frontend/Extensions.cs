using Cqrs;
using EventStore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using ReadModel;
using ReadModel.InMemory;
using Todo;

namespace Frontend
{
    public static class Extensions
    {
        public static void AddCqrs(this IServiceCollection self)
        {
            var eventStore = BuildEventStore();
            var readModel = BuildReadModel();

            var messageDispatcher = new MessageDispatcher(eventStore);
            messageDispatcher.ScanInstance(new TodoAggregate());
            messageDispatcher.ScanInstance(new EventConsumer(readModel));

            self
                .AddSingleton(_ => messageDispatcher)
                .AddSingleton<ITodoList>(_ => readModel);
        }

        private static IEventStore BuildEventStore() =>
            new InMemoryEventStore();

        private static InMemoryTodoList BuildReadModel() =>
            new InMemoryTodoList();
    }
}
