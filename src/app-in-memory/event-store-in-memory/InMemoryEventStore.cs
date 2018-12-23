using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cqrs;

namespace EventStore.InMemory
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly ConcurrentDictionary<Guid, Stream> store;

        public InMemoryEventStore()
        {
            store = new ConcurrentDictionary<Guid, Stream>();
        }

        public Task<IEnumerable<object>> LoadEventsForAsync<TAggregate>(Guid id)
        {
            // Get the current event stream. Note that we never mutate the
            // events array so it's safe to return the real thing.
            IEnumerable<object> events = store.TryGetValue(id, out var stream)
                ? stream.Events
                : new object[0];

            return Task.FromResult(events);
        }

        public Task SaveEventsForAsync<TAggregate>(Guid aggregateId, int version, object[] newEvents)
        {
            // Get or create stream
            var stream = store.GetOrAdd(aggregateId, _ => new Stream());

            // We'll use a lock-free algorithm for the update
            while (true)
            {
                // Read the current event list
                var events = stream.Events;

                // Ensure no events persisted since us
                // TODO: The version is not part of the event, only the aggregate
                var currentVersion = events
                    .Select(e => e.Version)
                    .DefaultIfEmpty(0)
                    .Max();

                if (currentVersion != version) throw new Exception("Concurrency conflict; cannot persist these events");

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

            return Task.CompletedTask;
        }

        private class Stream
        {
            public object[] Events;
        }
    }
}
