using System;
using Eventstore.InMemory;
using Shouldly;
using Xunit;

namespace Eventstore
{
    public class InMemoryEventstoreShould
    {
        private readonly InMemoryEventstore inMemoryEventstore;
        private readonly Guid id;

        public InMemoryEventstoreShould()
        {
            inMemoryEventstore = new InMemoryEventstore();
            id = Guid.NewGuid();
        }

        [Fact]
        public void LoadNoEventsForNewAggregate()
        {
            // Act
            var actual = inMemoryEventstore.LoadEventsFor<SomeAggregate>(id);

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

            inMemoryEventstore.SaveEventsFor<SomeAggregate>(id, 0, events);

            // Act
            var actual = inMemoryEventstore.LoadEventsFor<SomeAggregate>(id);

            // Assert
            actual.ShouldBe(events);
        }

        private class SomeAggregate
        {
        }
    }
}
