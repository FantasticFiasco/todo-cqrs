using System;
using System.Collections;

namespace Cqrs
{
    /// <summary>
    /// Aggregate base class, which factors out some common infrastructure that
    /// all aggregates have (ID and event application).
    /// </summary>
    public class Aggregate
    {
        /// <summary>
        /// The unique ID of the aggregate.
        /// </summary>
        public Guid Id { get; internal set; }

        /// <summary>
        /// The number of events loaded into this aggregate.
        /// </summary>
        public int EventsLoaded { get; private set; }

        /// <summary>
        /// Enumerates the supplied events and applies them in order to the aggregate.
        /// </summary>
        public void ApplyEvents(IEnumerable events)
        {
            foreach (var e in events)
            {
                GetType()
                    .GetMethod("ApplyOneEvent")
                    ?.MakeGenericMethod(e.GetType())
                    .Invoke(this, new[] { e });
            }
        }

        /// <summary>
        /// Applies a single event to the aggregate.
        /// </summary>
        public void ApplyOneEvent<TEvent>(TEvent ev)
        {
            var applier = this as IApplyEvent<TEvent>;

            if (applier == null) throw new InvalidOperationException($"Aggregate {GetType().Name} does not know how to apply event {ev.GetType().Name}");

            applier.Apply(ev);
            EventsLoaded++;
        }
    }
}
