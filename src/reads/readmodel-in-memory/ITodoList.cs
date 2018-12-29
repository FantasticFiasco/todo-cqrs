using System;

namespace ReadModel.InMemory
{
    public interface ITodoList
    {
        TodoItem[] GetAll();

        TodoItem Get(Guid id);
    }
}
