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
        public void PublishItemAddedGivenAddItem()
        {
            var addItem = new AddItem
            {
                Id = Guid.NewGuid(),
                Title = "Buy cheese"
            };

            Test(
                Given(),
                When(addItem),
                Then(new ItemAdded
                {
                    Id = addItem.Id,
                    Title = addItem.Title
                }));
        }
    }
}
