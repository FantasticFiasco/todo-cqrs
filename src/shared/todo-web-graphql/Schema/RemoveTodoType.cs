using GraphQL.Types;

namespace Todo.Web.GraphQL.Schema
{
    public class RemoveTodoType : ObjectGraphType<RemoveTodo>
    {
        public RemoveTodoType()
        {
            Name = "RemoveTodo";

            Field(self => self.Id, type: typeof(IdGraphType)).Description("The id of the todo.");
        }
    }
}
