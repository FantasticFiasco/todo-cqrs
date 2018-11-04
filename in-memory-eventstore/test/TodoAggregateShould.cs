using System;
using Cqrs;
using Todo.Commands;
using Todo.Events;
using Xunit;

namespace InMemoryEventstore.Test
{
    public class TodoAggregateShould : BddTest<TodoAggregate>
    {
        [Fact]
        public void AddItem()
        {
            Test(
                Given(),
                When(new AddItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Buy cheese"
                }),
                Then(new ItemAdded
                {
                    Id = Guid.NewGuid(),
                    Title = "Buy cheese"
                }));
        }
    }
}
