using System;
using System.Collections.Generic;
using Cqrs;

namespace EventStore.NoSql
{
    public class NoSqlEventStore : IEventStore
    {
        public IEnumerable<object> LoadEventsFor<TAggregate>(Guid id) => throw new NotImplementedException();

        public void SaveEventsFor<TAggregate>(Guid id, int eventsLoaded, object[] newEvents) => throw new NotImplementedException();
    }
}
