using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;
using Todo.Events;
using Xunit;

namespace ReadModel.InMemory
{
    public class EventConsumerShould
    {
        private readonly InMemoryTodoList todoList;
        private readonly EventConsumer eventConsumer;

        public EventConsumerShould()
        {
            todoList = new InMemoryTodoList(NullLogger<InMemoryTodoList>.Instance);
            eventConsumer = new EventConsumer(todoList, NullLogger<EventConsumer>.Instance);
        }

        [Fact]
        public async Task HandleAddedTodoItem()
        {
            // Act
            eventConsumer.Publish(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));

            // Assert
            (await todoList.GetAllAsync()).ShouldBe(new[] { ObjectMother.BuyCheese });
        }

        [Fact]
        public async Task HandleAddedTodoItems()
        {
            // Act
            eventConsumer.Publish(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            eventConsumer.Publish(new TodoAdded(ObjectMother.WashCar.Id, ObjectMother.WashCar.Title));

            // Assert
            (await todoList.GetAllAsync()).ShouldBe(new[] { ObjectMother.BuyCheese, ObjectMother.WashCar });
        }

        [Fact]
        public async Task HandleCompletedTodoItem()
        {
            // Arrange
            eventConsumer.Publish(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            eventConsumer.Publish(new TodoAdded(ObjectMother.WashCar.Id, ObjectMother.WashCar.Title));

            // Act
            eventConsumer.Publish(new TodoCompleted(ObjectMother.BuyCheese.Id));

            // Assert
            (await todoList.GetAllAsync()).ShouldBe(new[] { ObjectMother.BuyCheese.ButCompleted(), ObjectMother.WashCar });
        }

        [Fact]
        public async Task HandleIncompletedTodoItem()
        {
            // Arrange
            eventConsumer.Publish(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            eventConsumer.Publish(new TodoAdded(ObjectMother.WashCar.Id, ObjectMother.WashCar.Title));
            eventConsumer.Publish(new TodoCompleted(ObjectMother.BuyCheese.Id));

            // Act
            eventConsumer.Publish(new TodoIncompleted(ObjectMother.BuyCheese.Id));

            // Assert
            (await todoList.GetAllAsync()).ShouldBe(new[] { ObjectMother.BuyCheese, ObjectMother.WashCar });
        }

        [Fact]
        public async Task HandleRemoveIncompletedTodoItem()
        {
            // Arrange
            eventConsumer.Publish(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));

            // Act
            eventConsumer.Publish(new TodoRemoved(ObjectMother.BuyCheese.Id));

            // Assert
            (await todoList.GetAllAsync()).ShouldBeEmpty();
        }

        [Fact]
        public async Task HandleRemoveCompletedTodoItem()
        {
            // Arrange
            eventConsumer.Publish(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));
            eventConsumer.Publish(new TodoCompleted(ObjectMother.BuyCheese.Id));

            // Act
            eventConsumer.Publish(new TodoRemoved(ObjectMother.BuyCheese.Id));

            // Assert
            (await todoList.GetAllAsync()).ShouldBeEmpty();
        }

        [Fact]
        public async Task HandleRenamedTodoItem()
        {
            // Arrange
            eventConsumer.Publish(new TodoAdded(ObjectMother.BuyCheese.Id, ObjectMother.BuyCheese.Title));

            var newTitle = "Apply for 6-month tax extension";

            // Act
            eventConsumer.Publish(new TodoRenamed(ObjectMother.BuyCheese.Id, newTitle));

            // Assert
            (await todoList.GetAllAsync()).ShouldBe(new[] { ObjectMother.BuyCheese.ButWithTitle(newTitle) });
        }
    }
}
