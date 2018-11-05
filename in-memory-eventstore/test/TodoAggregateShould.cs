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
        public void ReturnTodoAddedGivenAddTodo()
        {
            var addItem = new AddTodo
            {
                Id = Guid.NewGuid(),
                Title = "Buy cheese"
            };

            Test(
                Given(),
                When(addItem),
                Then(new TodoAdded
                {
                    Id = addItem.Id,
                    Title = addItem.Title
                }));
        }
    }
}
