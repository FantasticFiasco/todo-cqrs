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
            // Get the current event stream
            var events = store.TryGetValue(id, out var stream)
                ? stream.Events
                : new List<object>();

            logger.LogInformation("Loaded {count} events for aggregate {id}", events.Count, id);

            return events;
        }

        public void SaveEventsFor<TAggregate>(Guid id, int eventsLoaded, object[] newEvents)
        {
            logger.LogInformation("Save {count} event(s) for aggregate {id}", newEvents.Length, id);

            // Get or create stream
            var stream = store.GetOrAdd(id, new Stream());

            lock (stream)
            {
                // Ensure no events persisted since us
                if (stream.Events.Count != eventsLoaded) throw new Exception("Concurrency conflict; cannot persist these events");

                stream.Events.AddRange(newEvents);
            }
        }

        private class Stream
        {
            public List<object> Events { get; } = new List<object>();
        }
    }
}
