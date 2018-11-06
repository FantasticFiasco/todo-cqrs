using System;
using System.Collections.Generic;

namespace Cqrs
{
    public interface IEventstore
    {
        object[] LoadEventsFor<TAggregate>(Guid id);

        void SaveEventsFor<TAggregate>(Guid id, int eventsLoaded, object[] newEvents);
    }
}
