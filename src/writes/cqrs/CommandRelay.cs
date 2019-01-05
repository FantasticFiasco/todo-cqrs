using System;
using System.Collections.Generic;
using System.Linq;

namespace Cqrs
{
    /// <summary>
    /// Class responsible for relying sent commands to registered aggregate handlers.
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

        /// <inheritdoc />
        public void RegisterHandlerFor<TCommand, TAggregate>()
            where TAggregate : Aggregate, new()
        {
            if (commandHandlers.ContainsKey(typeof(TCommand))) throw new Exception($"Command handler already registered for {typeof(TCommand).Name}");

            commandHandlers.Add(
                typeof(TCommand),
                command =>
                    {
                        // Create an empty aggregate
                        var aggregate = new TAggregate
                        {
                            Id = ((dynamic)command).Id
                        };

                        // Load the aggregate with events
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

        /// <inheritdoc />
        public void RegisterHandlersFor<TAggregate>()
            where TAggregate : Aggregate, new()
        {
            var handlers =
                from @interface in typeof(TAggregate).GetInterfaces()
                where @interface.IsGenericType
                where @interface.GetGenericTypeDefinition() == typeof(IHandleCommand<>)
                select new
                {
                    CommandType = @interface.GetGenericArguments().First(),
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void RegisterPublishersFor(object instance)
        {
            var publishers =
                from @interface in instance.GetType().GetInterfaces()
                where @interface.IsGenericType
                where @interface.GetGenericTypeDefinition() == typeof(IPublisher<>)
                select @interface.GetGenericArguments().First();

            foreach (var publisher in publishers)
            {
                GetType()
                    .GetMethod(nameof(RegisterPublisherFor))
                    ?.MakeGenericMethod(publisher)
                    .Invoke(this, new[] { instance });
            }
        }

        /// <inheritdoc />
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
