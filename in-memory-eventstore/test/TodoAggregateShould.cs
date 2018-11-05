using System;
using Cqrs;
using Todo.Commands;
using Todo.Events;
using Xunit;

namespace InMemoryEventstore.Test
{
    public class TodoAggregateShould : BddTest<TodoAggregate>
    {
        private readonly Guid id;
        private readonly string title;
        private readonly string newTitle;

        public TodoAggregateShould()
        {
            id = Guid.NewGuid();
            title = "Buy cheese";
            newTitle = "Apply for 6-month tax extension";
        }

        [Fact]
        public void ReturnTodoAddedGivenAddTodo()
        {
            Test(
                Given(),
                When(new AddTodo(id, title)),
                Then(new TodoAdded(id, title)));
        }

        [Fact]
        public void ReturnTodoRenamedGivenRenameTodo()
        {
            Test(
                Given(new TodoAdded(id, title)),
                When(new RenameTodo(id, newTitle)),
                Then(new TodoRenamed(id, newTitle)));
        }

        [Fact]
        public void ThrowExceptionOnRenameTodoGivenTodoRemoved()
        {
            Test(
                Given(
                    new TodoAdded(id, title),
                    new TodoRemoved(id)),
                When(new RenameTodo(id, newTitle)),
                ThenFailWith<TodoRemovedException>());
        }

        [Fact]
        public void ReturnTodoCompletedGivenCompleteTodo()
        {
            Test(
                Given(new TodoAdded(id, title)),
                When(new CompleteTodo(id)),
                Then(new TodoCompleted(id)));
        }

        [Fact]
        public void ThrowExceptionOnCompleteTodoGivenTodoRemoved()
        {
            Test(
                Given(
                    new TodoAdded(id, title),
                    new TodoRemoved(id)),
                When(new CompleteTodo(id)),
                ThenFailWith<TodoRemovedException>());
        }

        [Fact]
        public void ReturnTodoIncompletedGivenIncompleteTodo()
        {
            Test(
                Given(
                    new TodoAdded(id, title),
                    new TodoCompleted(id)),
                When(new IncompleteTodo(id)),
                Then(new TodoIncompleted(id)));
        }

        [Fact]
        public void ThrowExceptionOnIncompleteTodoGivenTodoRemoved()
        {
            Test(
                Given(
                    new TodoAdded(id, title),
                    new TodoRemoved(id)),
                When(new IncompleteTodo(id)),
                ThenFailWith<TodoRemovedException>());
        }

        [Fact]
        public void ReturnTodoRemovedGivenRemoveIncompletedTodo()
        {
            Test(
                Given(new TodoAdded(id, title)),
                When(new RemoveTodo(id)),
                Then(new TodoRemoved(id)));
        }

        [Fact]
        public void ReturnTodoRemovedGivenRemoveCompletedTodo()
        {
            Test(
                Given(
                    new TodoAdded(id, title),
                    new TodoCompleted(id)),
                When(new RemoveTodo(id)),
                Then(new TodoRemoved(id)));
        }
    }
}
