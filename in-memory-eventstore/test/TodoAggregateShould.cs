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
            var addTodo = new AddTodo(Guid.NewGuid(), "Buy cheese");

            Test(
                Given(),
                When(addTodo),
                Then(new TodoAdded(addTodo.Id, addTodo.Title)));
        }

        [Fact]
        public void ReturnTodoCompletedGivenCompleteTodo()
        {
            var todoAdded = CreateTodoAdded("File taxes");

            Test(
                Given(todoAdded),
                When(new CompleteTodo(todoAdded.Id)),
                Then(new TodoCompleted(todoAdded.Id)));
        }

        [Fact]
        public void ReturnTodoIncompletedGivenIncompleteTodo()
        {
            var todoAdded = CreateTodoAdded("File taxes");

            Test(
                Given(
                    todoAdded,
                    new TodoCompleted(todoAdded.Id)),
                When(new IncompleteTodo(todoAdded.Id)),
                Then(new TodoIncompleted(todoAdded.Id)));
        }

        [Fact]
        public void ReturnTodoRemovedGivenRemoveIncompleteTodo()
        {
            var todoAdded = CreateTodoAdded("File taxes");

            Test(
                Given(todoAdded),
                When(new RemoveTodo(todoAdded.Id)),
                Then(new TodoRemoved(todoAdded.Id)));
        }

        [Fact]
        public void ReturnTodoRemovedGivenRemoveCompleteTodo()
        {
            var todoAdded = CreateTodoAdded("File taxes");

            Test(
                Given(todoAdded, new CompleteTodo(todoAdded.Id)),
                When(new RemoveTodo(todoAdded.Id)),
                Then(new TodoRemoved(todoAdded.Id)));
        }

        [Fact]
        public void ReturnTodoRemovedGivenRenameTodo()
        {
            var todoAdded = CreateTodoAdded("File taxes");
            var renameTodo = new RenameTodo(todoAdded.Id, "Apply for 6-month tax extension");

            Test(
                Given(todoAdded),
                When(renameTodo),
                Then(new TodoRenamed(renameTodo.Id, renameTodo.NewTitle)));
        }

        private static TodoAdded CreateTodoAdded(string title) =>
            new TodoAdded(Guid.NewGuid(), title);
    }
}
