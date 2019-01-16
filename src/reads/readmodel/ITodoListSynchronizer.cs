using System;

namespace ReadModel
{
    public interface ITodoListSynchronizer
    {
        void Add(Guid id, string title);

        void Rename(Guid id, string newTitle);

        void SetCompleted(Guid id, bool isCompleted);

        void Remove(Guid id);
    }
}
