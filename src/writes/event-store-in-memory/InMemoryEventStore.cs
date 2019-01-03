using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Cqrs;
using Microsoft.Extensions.Logging;

namespace EventStore.InMemory
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly ConcurrentDictionary<Guid, Stream> store;
        private readonly ILogger<InMemoryEventStore> logger;

        public InMemoryEventStore(ILogger<InMemoryEventStore> logger)
        {
            store = new ConcurrentDictionary<Guid, Stream>();
            this.logger = logger;
        }

        public IEnumerable<object> LoadEventsFor<TAggregate>(Guid id)
        {
            // Get the current event stream. Note that we never mutate the
            // events array so it's safe to return the real thing.
            var events = store.TryGetValue(id, out var stream)
                ? stream.Events
                : new object[0];

            logger.LogInformation("Loaded {count} events for aggregate {id}", events.Length, id);

            return events;
        }

        public void SaveEventsFor<TAggregate>(Guid id, int eventsLoaded, object[] newEvents)
        {
            logger.LogInformation("Save {count} event(s) for aggregate {id}", newEvents.Length, id);

            // Get or create stream
            var stream = store.GetOrAdd(id, _ => new Stream());

            // We'll use a lock-free algorithm for the update
            while (true)
            {
                // Read the current event list
                var events = stream.Events;

                // Ensure no events persisted since us
                var previousEvents = events?.Length ?? 0;

                if (previousEvents != eventsLoaded) throw new Exception("Concurrency conflict; cannot persist these events");

                // Create a new event list with existing ones plus our new
                // ones (making new important for lock free algorithm!)
                var newEventList = events == null
                    ? new List<object>()
                    : new List<object>((object[])events.Clone());

                newEventList.AddRange(newEvents);

                // Try to put the new event list in place atomically
                if (Interlocked.CompareExchange(ref stream.Events, newEventList.ToArray(), events) == events)
                {
                    break;
                }
            }
        }

        private class Stream
        {
            public object[] Events;
        }
    }
}
