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
                When(new AddTodo { Id = id, Title = title }),
                Then(new TodoAdded { Id = id, Title = title }));
        }

        [Fact]
        public void ThrowExceptionOnAddTodoGivenTodoAdded()
        {
            Test(
                Given(new TodoAdded { Id = id, Title = title }),
                When(new AddTodo { Id = id, Title = title }),
                ThenFailWith<TodoAlreadyExistsException>());
        }

        [Fact]
        public void ReturnTodoRenamedGivenRenameTodo()
        {
            Test(
                Given(new TodoAdded { Id = id, Title = title }),
                When(new RenameTodo { Id = id, NewTitle = newTitle }),
                Then(new TodoRenamed { Id = id, NewTitle = newTitle }));
        }

        [Fact]
        public void ThrowExceptionOnRenameTodoGivenNotAdded()
        {
            Test(
                Given(),
                When(new RenameTodo { Id = id, NewTitle = newTitle }),
                ThenFailWith<TodoDoesNotExistException>());
        }


        [Fact]
        public void ThrowExceptionOnRenameTodoGivenTodoRemoved()
        {
            Test(
                Given(
                    new TodoAdded { Id = id, Title = title },
                    new TodoRemoved { Id = id }),
                When(new RenameTodo { Id = id, NewTitle = newTitle }),
                ThenFailWith<TodoDoesNotExistException>());
        }

        [Fact]
        public void ReturnTodoCompletedGivenCompleteTodo()
        {
            Test(
                Given(new TodoAdded { Id = id, Title = title }),
                When(new CompleteTodo { Id = id }),
                Then(new TodoCompleted { Id = id }));
        }

        [Fact]
        public void ThrowExceptionOnCompleteTodoGivenNotAdded()
        {
            Test(
                Given(),
                When(new CompleteTodo { Id = id }),
                ThenFailWith<TodoDoesNotExistException>());
        }

        [Fact]
        public void ThrowExceptionOnCompleteTodoGivenTodoRemoved()
        {
            Test(
                Given(
                    new TodoAdded { Id = id, Title = title },
                    new TodoRemoved { Id = id }),
                When(new CompleteTodo { Id = id }),
                ThenFailWith<TodoDoesNotExistException>());
        }

        [Fact]
        public void ReturnTodoIncompletedGivenIncompleteTodo()
        {
            Test(
                Given(
                    new TodoAdded { Id = id, Title = title},
                    new TodoCompleted { Id = id }),
                When(new IncompleteTodo { Id = id }),
                Then(new TodoIncompleted { Id = id }));
        }

        [Fact]
        public void ThrowExceptionOnIncompleteTodoGivenNotAdded()
        {
            Test(
                Given(),
                When(new IncompleteTodo { Id = id }),
                ThenFailWith<TodoDoesNotExistException>());
        }

        [Fact]
        public void ThrowExceptionOnIncompleteTodoGivenTodoRemoved()
        {
            Test(
                Given(
                    new TodoAdded { Id = id, Title = title },
                    new TodoRemoved { Id = id }),
                When(new IncompleteTodo { Id = id }),
                ThenFailWith<TodoDoesNotExistException>());
        }

        [Fact]
        public void ReturnTodoRemovedGivenRemoveIncompletedTodo()
        {
            Test(
                Given(new TodoAdded { Id = id, Title = title }),
                When(new RemoveTodo { Id = id }),
                Then(new TodoRemoved { Id = id }));
        }

        [Fact]
        public void ReturnTodoRemovedGivenRemoveCompletedTodo()
        {
            Test(
                Given(
                    new TodoAdded { Id = id, Title = title },
                    new TodoCompleted { Id = id }),
                When(new RemoveTodo { Id = id }),
                Then(new TodoRemoved { Id = id }));
        }

        [Fact]
        public void ThrowExceptionOnRemoveTodoGivenNotAdded()
        {
            Test(
                Given(),
                When(new RemoveTodo { Id = id }),
                ThenFailWith<TodoDoesNotExistException>());
        }

        [Fact]
        public void ThrowExceptionOnRemoveTodoGivenTodoRemoved()
        {
            Test(
                Given(
                    new TodoAdded { Id = id, Title = title },
                    new TodoRemoved { Id = id }),
                When(new RemoveTodo { Id = id }),
                ThenFailWith<TodoDoesNotExistException>());
        }
    }
}
