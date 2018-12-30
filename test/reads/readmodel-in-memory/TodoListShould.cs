using System.Threading.Tasks;
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
        public async Task HandleAddedTodoItem()
        {
            // Act
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));

            // Assert
            (await inMemoryTodoList.GetAllAsync()).ShouldBe(new[] { ObjectMother.BuyCheese });
        }

        [Fact]
        public async Task HandleAddedTodoItems()
        {
            // Act
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.WashCar.Id, ObjectMother.WashCar.Title));

            // Assert
            (await inMemoryTodoList.GetAllAsync()).ShouldBe(new[] { ObjectMother.BuyCheese, ObjectMother.WashCar });
        }

        [Fact]
        public async Task HandleCompletedTodoItem()
        {
            // Arrange
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.WashCar.Id, ObjectMother.WashCar.Title));

            // Act
            inMemoryTodoList.Handle(new TodoCompleted(ObjectMother.BuyCheese.Id));

            // Assert
            (await inMemoryTodoList.GetAllAsync()).ShouldBe(new[] { ObjectMother.BuyCheese.ButCompleted(), ObjectMother.WashCar });
        }

        [Fact]
        public async Task HandleIncompletedTodoItem()
        {
            // Arrange
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.WashCar.Id, ObjectMother.WashCar.Title));
            inMemoryTodoList.Handle(new TodoCompleted(ObjectMother.BuyCheese.Id));

            // Act
            inMemoryTodoList.Handle(new TodoIncompleted(ObjectMother.BuyCheese.Id));

            // Assert
            (await inMemoryTodoList.GetAllAsync()).ShouldBe(new[] { ObjectMother.BuyCheese, ObjectMother.WashCar });
        }

        [Fact]
        public async Task HandleRemoveIncompletedTodoItem()
        {
            // Arrange
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));

            // Act
            inMemoryTodoList.Handle(new TodoRemoved(ObjectMother.BuyCheese.Id));

            // Assert
            (await inMemoryTodoList.GetAllAsync()).ShouldBeEmpty();
        }

        [Fact]
        public async Task HandleRemoveCompletedTodoItem()
        {
            // Arrange
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            inMemoryTodoList.Handle(new TodoCompleted(ObjectMother.BuyCheese.Id));

            // Act
            inMemoryTodoList.Handle(new TodoRemoved(ObjectMother.BuyCheese.Id));

            // Assert
            (await inMemoryTodoList.GetAllAsync()).ShouldBeEmpty();
        }

        [Fact]
        public async Task HandleRenamedTodoItem()
        {
            // Arrange
            inMemoryTodoList.Handle(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));

            var newTitle = "Apply for 6-month tax extension";

            // Act
            inMemoryTodoList.Handle(new TodoRenamed(ObjectMother.BuyCheese.Id, newTitle));

            // Assert
            (await inMemoryTodoList.GetAllAsync()).ShouldBe(new[] { ObjectMother.BuyCheese.ButWithTitle(newTitle) });
        }
    }
}
