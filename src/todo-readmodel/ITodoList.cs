namespace Todo.ReadModel
{
    public interface ITodoList
    {
        TodoItem[] GetAll();

        TodoItem Get(string id);
    }
}
