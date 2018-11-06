using System.Linq;
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
        public void AddTodoItemGivenEmpty()
        {
            // Arrange
            todoList.Handle(new TodoAdded(BuyCheese.Id, BuyCheese.Title));

            // Act
            var actual = todoList.GetAll();

            // Assert
            actual.ShouldBe(new[] { BuyCheese });
        }

        [Fact]
        public void AddTodoItemsGivenEmpty()
        {
            // Arrange
            todoList.Handle(new TodoAdded(BuyCheese.Id, BuyCheese.Title));
            todoList.Handle(new TodoAdded(WashCar.Id, WashCar.Title));

            // Act
            var actual = todoList.GetAll();

            // Assert
            actual.ShouldBe(new[] { BuyCheese, WashCar });
        }

        [Fact]
        public void CompleteTodoItem()
        {
            // Arrange
            todoList.Handle(new TodoAdded(BuyCheese.Id, BuyCheese.Title));
            todoList.Handle(new TodoAdded(WashCar.Id, WashCar.Title));
            todoList.Handle(new TodoCompleted(BuyCheese.Id));

            // Act
            var actual = todoList.GetAll();

            // Assert
            actual.Length.ShouldBe(2);

            actual[0].IsCompleted.ShouldBeTrue();
            actual[1].IsCompleted.ShouldBeFalse();
        }
    }
}
