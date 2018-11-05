using Cqrs;
using Todo.Events;

namespace Todo.ReadModel
{
    public class TodoList :
        ISubscribeTo<TodoAdded>,
        ISubscribeTo<TodoRenamed>,
        ISubscribeTo<TodoCompleted>,
        ISubscribeTo<TodoIncompleted>,
        ISubscribeTo<TodoRemoved>
    {
        public void Handle(TodoAdded e) => throw new System.NotImplementedException();

        public void Handle(TodoRenamed e) => throw new System.NotImplementedException();

        public void Handle(TodoCompleted e) => throw new System.NotImplementedException();

        public void Handle(TodoIncompleted e) => throw new System.NotImplementedException();

        public void Handle(TodoRemoved e) => throw new System.NotImplementedException();
    }
}
