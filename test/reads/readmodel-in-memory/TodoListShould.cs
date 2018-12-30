using Shouldly;
using Todo.Events;
using Xunit;

namespace ReadModel.InMemory
{
    public class TodoListShould
    {
        private readonly InMemoryTodoList inMemoryTodoList;

        public TodoListShould()
        {
            inMemoryTodoList = new InMemoryTodoList();
        }

        [Fact]
        public void HandleAddedTodoItem()
        {
            // Act
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));

            // Assert
            inMemoryTodoList.GetAll().ShouldBe(new[] { ObjectMother.BuyCheese });
        }

        [Fact]
        public void HandleAddedTodoItems()
        {
            // Act
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.WashCar.Id, ObjectMother.WashCar.Title));

            // Assert
            inMemoryTodoList.GetAll().ShouldBe(new[] { ObjectMother.BuyCheese, ObjectMother.WashCar });
        }

        [Fact]
        public void HandleCompletedTodoItem()
        {
            // Arrange
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.WashCar.Id, ObjectMother.WashCar.Title));

            // Act
            inMemoryTodoList.Handle(new TodoCompleted(ObjectMother.BuyCheese.Id));

            // Assert
            inMemoryTodoList.GetAll().ShouldBe(new[] { ObjectMother.BuyCheese.ButCompleted(), ObjectMother.WashCar });
        }

        [Fact]
        public void HandleIncompletedTodoItem()
        {
            // Arrange
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.WashCar.Id, ObjectMother.WashCar.Title));
            inMemoryTodoList.Handle(new TodoCompleted(ObjectMother.BuyCheese.Id));

            // Act
            inMemoryTodoList.Handle(new TodoIncompleted(ObjectMother.BuyCheese.Id));

            // Assert
            inMemoryTodoList.GetAll().ShouldBe(new[] { ObjectMother.BuyCheese, ObjectMother.WashCar });
        }

        [Fact]
        public void HandleRemoveIncompletedTodoItem()
        {
            // Arrange
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));

            // Act
            inMemoryTodoList.Handle(new TodoRemoved(ObjectMother.BuyCheese.Id));

            // Assert
            inMemoryTodoList.GetAll().ShouldBeEmpty();
        }

        [Fact]
        public void HandleRemoveCompletedTodoItem()
        {
            // Arrange
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            inMemoryTodoList.Handle(new TodoCompleted(ObjectMother.BuyCheese.Id));

            // Act
            inMemoryTodoList.Handle(new TodoRemoved(ObjectMother.BuyCheese.Id));

            // Assert
            inMemoryTodoList.GetAll().ShouldBeEmpty();
        }

        [Fact]
        public void HandleRenamedTodoItem()
        {
            // Arrange
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));

            var newTitle = "Apply for 6-month tax extension";

            // Act
            inMemoryTodoList.Handle(new TodoRenamed(ObjectMother.BuyCheese.Id, newTitle));

            // Assert
            inMemoryTodoList.GetAll().ShouldBe(new[] { ObjectMother.BuyCheese.ButWithTitle(newTitle) });
        }
    }
}
