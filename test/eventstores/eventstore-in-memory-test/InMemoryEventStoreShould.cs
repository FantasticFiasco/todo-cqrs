using System;
using EventStore.InMemory;
using Shouldly;
using Xunit;

namespace EventStore
{
    public class InMemoryEventStoreShould
    {
        private readonly InMemoryEventStore inMemoryEventStore;
        private readonly Guid id;

        public InMemoryEventStoreShould()
        {
            inMemoryEventStore = new InMemoryEventStore();
            id = Guid.NewGuid();
        }

        [Fact]
        public void LoadNoEventsForNewAggregate()
        {
            // Act
            var actual = inMemoryEventStore.LoadEventsFor<SomeAggregate>(id);

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

            inMemoryEventStore.SaveEventsFor<SomeAggregate>(id, 0, events);

            // Act
            var actual = inMemoryEventStore.LoadEventsFor<SomeAggregate>(id);

            // Assert
            actual.ShouldBe(events);
        }

        private class SomeAggregate
        {
        }
    }
}
