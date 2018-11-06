using System;

namespace Todo.ReadModel
{
    public static class ObjectMother
    {
        public static readonly TodoItem BuyCheese = new TodoItem(Guid.NewGuid(), "Buy cheese");

        public static readonly TodoItem WashCar = new TodoItem(Guid.NewGuid(), "Wash the car");
    }
}
