using System;

namespace ReadModel.InMemory
{
    public static class ObjectMother
    {
        public static readonly TodoItem BuyCheese = new TodoItem(Guid.NewGuid(), "Buy cheese", false);

        public static readonly TodoItem WashCar = new TodoItem(Guid.NewGuid(), "Wash the car", false);

        public static TodoItem ButCompleted(this TodoItem self) =>
            new TodoItem(self.Id, self.Title, true);

        public static TodoItem ButWithTitle(this TodoItem self, string newTitle) =>
            new TodoItem(self.Id, newTitle, self.IsCompleted);
    }
}
