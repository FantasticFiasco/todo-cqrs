using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;
using Todo.Events;
using Xunit;

namespace ReadModel.InMemory
{
    public class InMemoryEventProcessorShould
    {
        private readonly InMemoryTodoList todoList;
        private readonly InMemoryEventProcessor inMemoryEventProcessor;

        public InMemoryEventProcessorShould()
        {
            todoList = new InMemoryTodoList(NullLogger<InMemoryTodoList>.Instance);
            inMemoryEventProcessor = new InMemoryEventProcessor(todoList, NullLogger<InMemoryEventProcessor>.Instance);
        }

        [Fact]
        public async Task HandleAddedTodoItem()
        {
            // Act
            inMemoryEventProcessor.Publish(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));

            // Assert
            (await todoList.GetAllAsync()).ShouldBe(new[] { ObjectMother.BuyCheese });
        }

        [Fact]
        public async Task HandleAddedTodoItems()
        {
            // Act
            inMemoryEventProcessor.Publish(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            inMemoryEventProcessor.Publish(new TodoAdded(ObjectMother.WashCar.Id, ObjectMother.WashCar.Title));

            // Assert
            (await todoList.GetAllAsync()).ShouldBe(new[] { ObjectMother.BuyCheese, ObjectMother.WashCar });
        }

        [Fact]
        public async Task HandleCompletedTodoItem()
        {
            // Arrange
            inMemoryEventProcessor.Publish(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            inMemoryEventProcessor.Publish(new TodoAdded(ObjectMother.WashCar.Id, ObjectMother.WashCar.Title));

            // Act
            inMemoryEventProcessor.Publish(new TodoCompleted(ObjectMother.BuyCheese.Id));

            // Assert
            (await todoList.GetAllAsync()).ShouldBe(new[] { ObjectMother.BuyCheese.ButCompleted(), ObjectMother.WashCar });
        }

        [Fact]
        public async Task HandleIncompletedTodoItem()
        {
            // Arrange
            inMemoryEventProcessor.Publish(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            inMemoryEventProcessor.Publish(new TodoAdded(ObjectMother.WashCar.Id, ObjectMother.WashCar.Title));
            inMemoryEventProcessor.Publish(new TodoCompleted(ObjectMother.BuyCheese.Id));

            // Act
            inMemoryEventProcessor.Publish(new TodoIncompleted(ObjectMother.BuyCheese.Id));

            // Assert
            (await todoList.GetAllAsync()).ShouldBe(new[] { ObjectMother.BuyCheese, ObjectMother.WashCar });
        }

        [Fact]
        public async Task HandleRemoveIncompletedTodoItem()
        {
            // Arrange
            inMemoryEventProcessor.Publish(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));

            // Act
            inMemoryEventProcessor.Publish(new TodoRemoved(ObjectMother.BuyCheese.Id));

            // Assert
            (await todoList.GetAllAsync()).ShouldBeEmpty();
        }

        [Fact]
        public async Task HandleRemoveCompletedTodoItem()
        {
            // Arrange
            inMemoryEventProcessor.Publish(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            inMemoryEventProcessor.Publish(new TodoCompleted(ObjectMother.BuyCheese.Id));

            // Act
            inMemoryEventProcessor.Publish(new TodoRemoved(ObjectMother.BuyCheese.Id));

            // Assert
            (await todoList.GetAllAsync()).ShouldBeEmpty();
        }

        [Fact]
        public async Task HandleRenamedTodoItem()
        {
            // Arrange
            inMemoryEventProcessor.Publish(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));

            var newTitle = "Apply for 6-month tax extension";

            // Act
            inMemoryEventProcessor.Publish(new TodoRenamed(ObjectMother.BuyCheese.Id, newTitle));

            // Assert
            (await todoList.GetAllAsync()).ShouldBe(new[] { ObjectMother.BuyCheese.ButWithTitle(newTitle) });
        }
    }
}
