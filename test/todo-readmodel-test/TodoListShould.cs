using System;
using Shouldly;
using Todo.Events;
using Xunit;
using static Todo.ReadModel.ObjectMother;

namespace Todo.ReadModel
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
            todoList.Handle(new TodoAdded(Guid.Parse(BuyCheese.Id), BuyCheese.Title));

            // Assert
            todoList.GetAll().ShouldBe(new[] { BuyCheese });
        }

        [Fact]
        public void HandleAddedTodoItems()
        {
            // Act
            todoList.Handle(new TodoAdded(Guid.Parse(BuyCheese.Id), BuyCheese.Title));
            todoList.Handle(new TodoAdded(Guid.Parse(WashCar.Id), WashCar.Title));

            // Assert
            todoList.GetAll().ShouldBe(new[] { BuyCheese, WashCar });
        }

        [Fact]
        public void HandleCompletedTodoItem()
        {
            // Arrange
            todoList.Handle(new TodoAdded(Guid.Parse(BuyCheese.Id), BuyCheese.Title));
            todoList.Handle(new TodoAdded(Guid.Parse(WashCar.Id), WashCar.Title));

            // Act
            todoList.Handle(new TodoCompleted(Guid.Parse(BuyCheese.Id)));

            // Assert
            todoList.GetAll().ShouldBe(new[] { BuyCheese.ButCompleted(), WashCar });
        }

        [Fact]
        public void HandleIncompletedTodoItem()
        {
            // Arrange
            todoList.Handle(new TodoAdded(Guid.Parse(BuyCheese.Id), BuyCheese.Title));
            todoList.Handle(new TodoAdded(Guid.Parse(WashCar.Id), WashCar.Title));
            todoList.Handle(new TodoCompleted(Guid.Parse(BuyCheese.Id)));

            // Act
            todoList.Handle(new TodoIncompleted(Guid.Parse(BuyCheese.Id)));

            // Assert
            todoList.GetAll().ShouldBe(new[] { BuyCheese, WashCar });
        }

        [Fact]
        public void HandleRemoveIncompletedTodoItem()
        {
            // Arrange
            todoList.Handle(new TodoAdded(Guid.Parse(BuyCheese.Id), BuyCheese.Title));

            // Act
            todoList.Handle(new TodoRemoved(Guid.Parse(BuyCheese.Id)));

            // Assert
            todoList.GetAll().ShouldBeEmpty();
        }

        [Fact]
        public void HandleRemoveCompletedTodoItem()
        {
            // Arrange
            todoList.Handle(new TodoAdded(Guid.Parse(BuyCheese.Id), BuyCheese.Title));
            todoList.Handle(new TodoCompleted(Guid.Parse(BuyCheese.Id)));

            // Act
            todoList.Handle(new TodoRemoved(Guid.Parse(BuyCheese.Id)));

            // Assert
            todoList.GetAll().ShouldBeEmpty();
        }

        [Fact]
        public void HandleRenamedTodoItem()
        {
            // Arrange
            todoList.Handle(new TodoAdded(Guid.Parse(BuyCheese.Id), BuyCheese.Title));

            var newTitle = "Apply for 6-month tax extension";

            // Act
            todoList.Handle(new TodoRenamed(Guid.Parse(BuyCheese.Id), newTitle));

            // Assert
            todoList.GetAll().ShouldBe(new[] { BuyCheese.ButWithTitle(newTitle) });
        }
    }
}
