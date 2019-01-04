using System;
using System.Collections.Generic;
using System.Linq;

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
    public class CommandRelay : ICommandRelay
    {
        private readonly IEventStore eventStore;
        private readonly Dictionary<Type, Action<object>> commandHandlers;
        private readonly Dictionary<Type, List<Action<object>>> eventPublishers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRelay"/> class.
        /// </summary>
        public CommandRelay(IEventStore eventStore)
        {
            this.eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));

            commandHandlers = new Dictionary<Type, Action<object>>();
            eventPublishers = new Dictionary<Type, List<Action<object>>>();
        }

        /// <summary>
        /// Registers an aggregate as being the handler for a particular command.
        /// </summary>
        /// <typeparam name="TCommand">
        /// The type of the command.
        /// </typeparam>
        /// <typeparam name="TAggregate">
        /// The type of the aggregate handling the particular command.
        /// </typeparam>
        public void RegisterHandlerFor<TCommand, TAggregate>()
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

                        // Publish the events
                        foreach (var e in events)
                        {
                            PublishEvent(e);
                        }
                    });
        }

        /// <summary>
        /// Registers an aggregate as being the handler for all its implementations of
        /// <see cref="IHandleCommand{T}"/>.
        /// </summary>
        /// <typeparam name="TAggregate">
        /// The type of the aggregate handling commands.
        /// </typeparam>
        public void RegisterHandlersFor<TAggregate>()
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
                    .GetMethod(nameof(RegisterHandlerFor))
                    ?.MakeGenericMethod(handler.CommandType, handler.AggregateType)
                    .Invoke(this, new object[] { });
            }
        }

        /// <summary>
        /// TODO: Rewrite
        ///
        /// Adds an object that subscribes to the specified event, by virtue of implementing the
        /// <see cref="IPublisher{TEvent}"/> interface.
        /// </summary>
        public void RegisterPublisherFor<TEvent>(IPublisher<TEvent> publisher)
        {
            var eventType = typeof(TEvent);

            if (!eventPublishers.TryGetValue(eventType, out var publishersOfEvent))
            {
                publishersOfEvent = new List<Action<object>>();
                eventPublishers.Add(eventType, publishersOfEvent);
            }

            publishersOfEvent.Add(e => publisher.Publish((TEvent)e));
        }


        /// <summary>
        /// TODO: Rewrite
        ///
        /// Looks at the specified object instance, examples what commands it handles
        /// or events it subscribes to, and registers it as a receiver/subscriber.
        /// </summary>
        public void RegisterPublishersFor(object instance)
        {
            var publishers =
                from @interface in instance.GetType().GetInterfaces()
                where @interface.IsGenericType
                where @interface.GetGenericTypeDefinition() == typeof(IPublisher<>)
                select @interface.GetGenericArguments()[0];

            foreach (var publisher in publishers)
            {
                GetType()
                    .GetMethod(nameof(RegisterPublisherFor))
                    ?.MakeGenericMethod(publisher)
                    .Invoke(this, new[] { instance });
            }
        }

        /// <summary>
        /// Invokes the intent of a command by sending it to its registered handler.
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

        private void PublishEvent(object e)
        {
            if (!eventPublishers.TryGetValue(e.GetType(), out var publishersOfEvent)){ return;}

            foreach (var publisherOfEvent in publishersOfEvent)
            {
                publisherOfEvent(e);
            }
        }
    }
}
