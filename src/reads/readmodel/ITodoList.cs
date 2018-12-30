using System;

namespace ReadModel
{
    public interface ITodoList
    {
        TodoItem[] GetAll();

        TodoItem Get(Guid id);
    }
}
