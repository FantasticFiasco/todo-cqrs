using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cqrs
{
    /// <summary>
    /// TODO: Rewrite
    ///
    /// This implements a basic message dispatcher, driving the overall command handling
    /// and event application/distribution process. It is suitable for a simple, single
    /// node application that can safely build its subscriber list at startup and keep
    /// it in memory. Depends on some kind of event storage mechanism.
    /// </summary>
    public class CommandRelay
    {
        private readonly IEventStore eventStore;
        private readonly Dictionary<Type, Action<object>> commandHandlers;
        private readonly Dictionary<Type, List<Action<object>>> eventSubscribers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRelay"/> class.
        /// </summary>
        public CommandRelay(IEventStore eventStore)
        {
            this.eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));

            commandHandlers = new Dictionary<Type, Action<object>>();
            eventSubscribers = new Dictionary<Type, List<Action<object>>>();
        }

        /// <summary>
        /// TODO: Rewrite
        ///
        /// Invokes command by sending it to its registered handler.
        /// </summary>
        /// <typeparam name="TCommand">
        /// The type of the command.
        /// </typeparam>
        /// <exception cref="Exception">
        /// No registered handler for command is found.
        /// </exception>
        public void SendCommand<TCommand>(TCommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            if (!commandHandlers.TryGetValue(typeof(TCommand), out var handler))
            {
                throw new Exception($"No command handler registered for {typeof(TCommand).Name}");
            }

            handler(command);
        }

        /// <summary>
        /// TODO: Rewrite
        ///
        /// Registers an aggregate as being the handler for a particular command.
        /// </summary>
        /// <typeparam name="TCommand">
        /// The type of the command.
        /// </typeparam>
        /// <typeparam name="TAggregate">
        /// The type of the aggregate handling the particular command.
        /// </typeparam>
        public void AddHandlerFor<TCommand, TAggregate>()
            where TAggregate : Aggregate, new()
        {
            if (commandHandlers.ContainsKey(typeof(TCommand))) throw new Exception($"Command handler already registered for {typeof(TCommand).Name}");

            commandHandlers.Add(
                typeof(TCommand),
                command =>
                    {
                        // Create an empty aggregate
                        var aggregate = new TAggregate();

                        // Load the aggregate with events
                        aggregate.Id = ((dynamic)command).Id;
                        aggregate.ApplyEvents(eventStore.LoadEventsFor<TAggregate>(aggregate.Id));

                        // With everything set up, we invoke the command handler, collecting the
                        // events that it produces
                        var events = ((IHandleCommand<TCommand>)aggregate)
                            .Handle((TCommand)command)
                            .Cast<object>()
                            .ToArray();

                        // Save events to the event store
                        eventStore.SaveEventsFor<TAggregate>(
                            aggregate.Id,
                            aggregate.Version,
                            events);

                        // Publish them to all subscribers
                        foreach (var e in events)
                        {
                            PublishEvent(e);
                        }
                    });
        }

        public void AddHandlersFor<TAggregate>()
            where TAggregate : Aggregate, new()
        {
            var handlers =
                from @interface in typeof(TAggregate).GetInterfaces()
                where @interface.IsGenericType
                where @interface.GetGenericTypeDefinition() == typeof(IHandleCommand<>)
                let arguments = @interface.GetGenericArguments()
                select new
                {
                    CommandType = arguments[0],
                    AggregateType = typeof(TAggregate)
                };

            foreach (var handler in handlers)
            {
                GetType()
                    .GetMethod(nameof(AddHandlerFor))
                    ?.MakeGenericMethod(handler.CommandType, handler.AggregateType)
                    .Invoke(this, new object[] { });
            }
        }

        /// <summary>
        /// TODO: Rewrite
        ///
        /// Adds an object that subscribes to the specified event, by virtue of implementing the
        /// <see cref="ISubscribeTo{T}"/> interface.
        /// </summary>
        public void AddSubscriberFor<TEvent>(ISubscribeTo<TEvent> subscriber)
        {
            var eventType = typeof(TEvent);

            if (!eventSubscribers.TryGetValue(eventType, out var subscribersToEvent))
            {
                subscribersToEvent = new List<Action<object>>();
                eventSubscribers.Add(eventType, subscribersToEvent);
            }

            subscribersToEvent.Add(e => subscriber.Handle((TEvent)e));
        }

        /// <summary>
        /// TODO: Rewrite
        ///
        /// Looks thorough the specified assembly for all public types that implement
        /// the IHandleCommand or ISubscribeTo generic interfaces. Registers each of
        /// the implementations as a command handler or event subscriber.
        /// </summary>
        public void ScanAssembly(Assembly ass)
        {
            // Scan for and register handlers.
            var handlers =
                from t in ass.GetTypes()
                from i in t.GetInterfaces()
                where i.IsGenericType
                where i.GetGenericTypeDefinition() == typeof(IHandleCommand<>)
                let args = i.GetGenericArguments()
                select new
                {
                    CommandType = args[0],
                    AggregateType = t
                };

            foreach (var h in handlers)
                GetType().GetMethod("AddHandlerFor")
                    ?.MakeGenericMethod(h.CommandType, h.AggregateType)
                    .Invoke(this, new object[] { });

            // Scan for and register subscribers.
            var subscriber =
                from t in ass.GetTypes()
                from i in t.GetInterfaces()
                where i.IsGenericType
                where i.GetGenericTypeDefinition() == typeof(ISubscribeTo<>)
                select new
                {
                    Type = t,
                    EventType = i.GetGenericArguments()[0]
                };

            foreach (var s in subscriber)
            {
                GetType()
                    .GetMethod("AddSubscriberFor")
                    ?.MakeGenericMethod(s.EventType)
                    .Invoke(this, new[] { CreateInstanceOf(s.Type) });
            }
        }

        /// <summary>
        /// TODO: Rewrite
        ///
        /// Looks at the specified object instance, examples what commands it handles
        /// or events it subscribes to, and registers it as a receiver/subscriber.
        /// </summary>
        public void ScanInstance(object instance)
        {
            // Scan for and register handlers.
            var handlers =
                from i in instance.GetType().GetInterfaces()
                where i.IsGenericType
                where i.GetGenericTypeDefinition() == typeof(IHandleCommand<>)
                let args = i.GetGenericArguments()
                select new
                {
                    CommandType = args[0],
                    AggregateType = instance.GetType()
                };

            foreach (var h in handlers)
            {
                GetType()
                    .GetMethod("AddHandlerFor")
                    ?.MakeGenericMethod(h.CommandType, h.AggregateType)
                    .Invoke(this, new object[] { });
            }

            // Scan for and register subscribers.
            var subscriber =
                from i in instance.GetType().GetInterfaces()
                where i.IsGenericType
                where i.GetGenericTypeDefinition() == typeof(ISubscribeTo<>)
                select i.GetGenericArguments()[0];

            foreach (var s in subscriber)
            {
                GetType()
                    .GetMethod("AddSubscriberFor")
                    ?.MakeGenericMethod(s)
                    .Invoke(this, new[] { instance });
            }
        }

        private void PublishEvent(object e)
        {
            if (!eventSubscribers.TryGetValue(e.GetType(), out var subscribersToEvent)){ return;}

            foreach (var subscriberToEvent in subscribersToEvent)
            {
                subscriberToEvent(e);
            }
        }

        private static object CreateInstanceOf(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
