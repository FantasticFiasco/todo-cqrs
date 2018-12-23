using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cqrs
{
    public interface IEventStore
    {
        Task<IEnumerable<object>> LoadEventsForAsync<TAggregate>(Guid id);

        Task SaveEventsForAsync<TAggregate>(Guid id, int version, object[] newEvents);
    }
}
