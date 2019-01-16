using System;

namespace Todo
{
    public class TodoDoesNotExistException : Exception
    {
        public TodoDoesNotExistException(Guid id)
            : base($"Todo with id {id} does not exist")
        {
        }
    }
}
