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
            var buyCheese = new AddTodo
            {
                Id = Guid.NewGuid(),
                Title = "Buy cheese"
            };

            Test(
                Given(),
                When(buyCheese),
                Then(new TodoAdded
                {
                    Id = buyCheese.Id,
                    Title = buyCheese.Title
                }));
        }

        [Fact]
        public void ReturnTodoCompletedGivenCompleteTodo()
        {
            var fileTaxes = CreateTodoAdded("File taxes");

            var completeTodo = new CompleteTodo
            {
                Id = fileTaxes.Id
            };


            Test(
                Given(fileTaxes),
                When(completeTodo),
                Then(new TodoCompleted
                    {
                        Id = completeTodo.Id
                    }));
        }

        private static TodoAdded CreateTodoAdded(string title)
        {
            return new TodoAdded
            {
                Id = Guid.NewGuid(),
                Title = title
            };
        }
    }
}
