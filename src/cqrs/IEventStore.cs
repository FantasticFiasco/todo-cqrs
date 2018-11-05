using System;
using System.Collections;

namespace Cqrs
{
    public interface IEventstore
    {
        IEnumerable LoadEventsFor<TAggregate>(Guid id);

        void SaveEventsFor<TAggregate>(Guid id, int eventsLoaded, ArrayList newEvents);
    }
}
