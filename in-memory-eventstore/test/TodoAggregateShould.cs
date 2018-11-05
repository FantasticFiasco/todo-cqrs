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
            var buyCheese = new AddTodo(Guid.NewGuid(), "Buy cheese");

            Test(
                Given(),
                When(buyCheese),
                Then(new TodoAdded(buyCheese.Id, buyCheese.Title)));
        }

        [Fact]
        public void ReturnTodoCompletedGivenCompleteTodo()
        {
            var taxesFiled = CreateTodoAdded("File taxes");

            Test(
                Given(taxesFiled),
                When(new CompleteTodo(taxesFiled.Id)),
                Then(new TodoCompleted(taxesFiled.Id)));
        }

        [Fact]
        public void ReturnTodoIncompletedGivenIncompleteTodo()
        {
            var taxesFiled = CreateTodoAdded("File taxes");

            Test(
                Given(
                    taxesFiled,
                    new TodoCompleted(taxesFiled.Id)),
                When(new IncompleteTodo(taxesFiled.Id)),
                Then(new TodoIncompleted(taxesFiled.Id)));
        }

        [Fact]
        public void ReturnTodoRemovedGivenRemoveIncompleteTodo()
        {
            var taxesFiled = CreateTodoAdded("File taxes");

            Test(
                Given(taxesFiled),
                When(new RemoveTodo(taxesFiled.Id)),
                Then(new TodoRemoved(taxesFiled.Id)));
        }

        [Fact]
        public void ReturnTodoRemovedGivenRemoveCompleteTodo()
        {
            var taxesFiled = CreateTodoAdded("File taxes");

            Test(
                Given(taxesFiled, new CompleteTodo(taxesFiled.Id)),
                When(new RemoveTodo(taxesFiled.Id)),
                Then(new TodoRemoved(taxesFiled.Id)));
        }

        [Fact]
        public void ReturnTodoRemovedGivenRenameTodo()
        {
            var taxesFiled = CreateTodoAdded("File taxes");
            var renameTodo = new RenameTodo(taxesFiled.Id, "Apply for 6-month tax extension");

            Test(
                Given(taxesFiled),
                When(renameTodo),
                Then(new TodoRenamed(renameTodo.Id, renameTodo.NewTitle)));
        }

        private static TodoAdded CreateTodoAdded(string title)
        {
            return new TodoAdded(Guid.NewGuid(), title);
        }
    }
}
