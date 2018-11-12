using GraphQL;
using GraphQL.Types;

namespace Todo.Web.GraphQL
{
    public class TodoSchema : Schema
    {
        public TodoSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            Query = resolver.Resolve<TodoQuery>();
            Mutation = resolver.Resolve<TodoMutation>();
        }
    }
}
