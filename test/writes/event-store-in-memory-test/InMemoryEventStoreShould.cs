using System;
using Shouldly;
using Xunit;

namespace EventStore.InMemory
{
    public class InMemoryEventStoreShould
    {
        private readonly InMemoryEventStore eventStore;
        private readonly Guid id;

        public InMemoryEventStoreShould()
        {
            eventStore = new InMemoryEventStore();
            id = Guid.NewGuid();
        }

        [Fact]
        public void LoadNoEventsForNewAggregate()
        {
            // Act
            var actual = eventStore.LoadEventsFor<SomeAggregate>(id);

            // Assert
            actual.ShouldBeEmpty();
        }

        [Fact]
        public void LoadEventsForExistingAggregate()
        {
            // Arrange
            var events = new object[]
            {
                new { Id = Guid.NewGuid() },
                new { Id = Guid.NewGuid() }
            };

            eventStore.SaveEventsFor<SomeAggregate>(id, 0, events);

            // Act
            var actual = eventStore.LoadEventsFor<SomeAggregate>(id);

            // Assert
            actual.ShouldBe(events);
        }

        private class SomeAggregate
        {
        }
    }
}
