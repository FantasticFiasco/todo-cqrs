using System.Collections.Generic;

namespace EventStore.InMemory
{
    internal class InMemoryStream
    {
        public List<object> Events { get; } = new List<object>();
    }
}
