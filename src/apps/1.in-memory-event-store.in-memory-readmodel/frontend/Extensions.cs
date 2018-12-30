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
            var messageDispatcher = BuildMessageDispatcher(eventStore, readModel);

            self
                .AddSingleton(_ => messageDispatcher)
                .AddSingleton(_ => readModel);
        }

        private static IEventStore BuildEventStore()
        {
            return new InMemoryEventStore();
        }

        private static ITodoList BuildReadModel()
        {
            return new InMemoryTodoList();
        }

        private static MessageDispatcher BuildMessageDispatcher(IEventStore eventStore, ITodoList readModel)
        {
            var messageDispatcher = new MessageDispatcher(eventStore);

            // Let the message dispatcher scan the aggregate and register the IHandleCommand implementations
            messageDispatcher.ScanInstance(new TodoAggregate());

            // Let the message dispatcher scan the event consumer and register the ISubscribeTo implementations
            messageDispatcher.ScanInstance(new EventConsumer((InMemoryTodoList)readModel));

            return messageDispatcher;
        }
    }
}
