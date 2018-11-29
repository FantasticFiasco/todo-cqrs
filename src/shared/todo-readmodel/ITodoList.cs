using System;

namespace Todo.ReadModel
{
    public interface ITodoList
    {
        TodoItem[] GetAll();

        TodoItem Get(Guid id);
    }
}
