using System;
using Cqrs;
using Todo.Events;
using Xunit;

namespace Todo.Test
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
        public void ThrowExceptionOnAddTodoGivenTodoAdded()
        {
            Test(
                Given(new TodoAdded(id, title)),
                When(new AddTodo(id, title)),
                ThenFailWith<TodoAlreadyExistsException>());
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
        public void ThrowExceptionOnRenameTodoGivenNotAdded()
        {
            Test(
                Given(),
                When(new RenameTodo(id, newTitle)),
                ThenFailWith<TodoDoesNotExistException>());
        }


        [Fact]
        public void ThrowExceptionOnRenameTodoGivenTodoRemoved()
        {
            Test(
                Given(
                    new TodoAdded(id, title),
                    new TodoRemoved(id)),
                When(new RenameTodo(id, newTitle)),
                ThenFailWith<TodoDoesNotExistException>());
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
        public void ThrowExceptionOnCompleteTodoGivenNotAdded()
        {
            Test(
                Given(),
                When(new CompleteTodo(id)),
                ThenFailWith<TodoDoesNotExistException>());
        }

        [Fact]
        public void ThrowExceptionOnCompleteTodoGivenTodoRemoved()
        {
            Test(
                Given(
                    new TodoAdded(id, title),
                    new TodoRemoved(id)),
                When(new CompleteTodo(id)),
                ThenFailWith<TodoDoesNotExistException>());
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
        public void ThrowExceptionOnIncompleteTodoGivenNotAdded()
        {
            Test(
                Given(),
                When(new IncompleteTodo(id)),
                ThenFailWith<TodoDoesNotExistException>());
        }

        [Fact]
        public void ThrowExceptionOnIncompleteTodoGivenTodoRemoved()
        {
            Test(
                Given(
                    new TodoAdded(id, title),
                    new TodoRemoved(id)),
                When(new IncompleteTodo(id)),
                ThenFailWith<TodoDoesNotExistException>());
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

        [Fact]
        public void ThrowExceptionOnRemoveTodoGivenNotAdded()
        {
            Test(
                Given(),
                When(new RemoveTodo(id)),
                ThenFailWith<TodoDoesNotExistException>());
        }

        [Fact]
        public void ThrowExceptionOnRemoveTodoGivenTodoRemoved()
        {
            Test(
                Given(
                    new TodoAdded(id, title),
                    new TodoRemoved(id)),
                When(new RemoveTodo(id)),
                ThenFailWith<TodoDoesNotExistException>());
        }
    }
}
