using System;
using System.Threading.Tasks;
using EventStore.InMemory;
using Shouldly;
using Xunit;

namespace EventStore
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
        public async Task LoadNoEventsForNewAggregate()
        {
            // Act
            var actual = await eventStore.LoadEventsForAsync<SomeAggregate>(id);

            // Assert
            actual.ShouldBeEmpty();
        }

        [Fact]
        public async Task LoadEventsForExistingAggregate()
        {
            // Arrange
            var events = new object[]
            {
                new { Id = Guid.NewGuid() },
                new { Id = Guid.NewGuid() }
            };

            await eventStore.SaveEventsForAsync<SomeAggregate>(id, 0, events);

            // Act
            var actual = await eventStore.LoadEventsForAsync<SomeAggregate>(id);

            // Assert
            actual.ShouldBe(events);
        }

        private class SomeAggregate
        {
        }
    }
}
