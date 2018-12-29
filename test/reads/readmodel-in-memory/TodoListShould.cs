using Shouldly;
using Todo.Events;
using Xunit;

namespace ReadModel.InMemory
{
    public class TodoListShould
    {
        private readonly TodoList todoList;

        public TodoListShould()
        {
            todoList = new TodoList();
        }

        [Fact]
        public void HandleAddedTodoItem()
        {
            // Act
            todoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));

            // Assert
            todoList.GetAll().ShouldBe(new[] { ObjectMother.BuyCheese });
        }

        [Fact]
        public void HandleAddedTodoItems()
        {
            // Act
            todoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            todoList.Handle(new TodoAdded(ObjectMother.WashCar.Id, ObjectMother.WashCar.Title));

            // Assert
            todoList.GetAll().ShouldBe(new[] { ObjectMother.BuyCheese, ObjectMother.WashCar });
        }

        [Fact]
        public void HandleCompletedTodoItem()
        {
            // Arrange
            todoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            todoList.Handle(new TodoAdded(ObjectMother.WashCar.Id, ObjectMother.WashCar.Title));

            // Act
            todoList.Handle(new TodoCompleted(ObjectMother.BuyCheese.Id));

            // Assert
            todoList.GetAll().ShouldBe(new[] { ObjectMother.BuyCheese.ButCompleted(), ObjectMother.WashCar });
        }

        [Fact]
        public void HandleIncompletedTodoItem()
        {
            // Arrange
            todoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            todoList.Handle(new TodoAdded(ObjectMother.WashCar.Id, ObjectMother.WashCar.Title));
            todoList.Handle(new TodoCompleted(ObjectMother.BuyCheese.Id));

            // Act
            todoList.Handle(new TodoIncompleted(ObjectMother.BuyCheese.Id));

            // Assert
            todoList.GetAll().ShouldBe(new[] { ObjectMother.BuyCheese, ObjectMother.WashCar });
        }

        [Fact]
        public void HandleRemoveIncompletedTodoItem()
        {
            // Arrange
            todoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));

            // Act
            todoList.Handle(new TodoRemoved(ObjectMother.BuyCheese.Id));

            // Assert
            todoList.GetAll().ShouldBeEmpty();
        }

        [Fact]
        public void HandleRemoveCompletedTodoItem()
        {
            // Arrange
            todoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            todoList.Handle(new TodoCompleted(ObjectMother.BuyCheese.Id));

            // Act
            todoList.Handle(new TodoRemoved(ObjectMother.BuyCheese.Id));

            // Assert
            todoList.GetAll().ShouldBeEmpty();
        }

        [Fact]
        public void HandleRenamedTodoItem()
        {
            // Arrange
            todoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));

            var newTitle = "Apply for 6-month tax extension";

            // Act
            todoList.Handle(new TodoRenamed(ObjectMother.BuyCheese.Id, newTitle));

            // Assert
            todoList.GetAll().ShouldBe(new[] { ObjectMother.BuyCheese.ButWithTitle(newTitle) });
        }
    }
}
