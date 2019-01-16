using GraphQL.Types;
using Todo;

namespace GraphQL.Configuration.Schema
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
