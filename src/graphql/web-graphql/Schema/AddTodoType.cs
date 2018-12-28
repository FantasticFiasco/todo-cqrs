using GraphQL.Types;
using Todo;

namespace GraphQL.Schema
{
    public class AddTodoType : ObjectGraphType<AddTodo>
    {
        public AddTodoType()
        {
            Name = "AddTodo";

            Field(self => self.Id, type: typeof(IdGraphType)).Description("The id of the todo.");
            Field(self => self.Title).Description("The title of the todo.");
        }
    }
}
