using GraphQL;

namespace Todo.Web.GraphQL.Schema
{
    public class TodoSchema : global::GraphQL.Types.Schema
    {
        public TodoSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            Query = resolver.Resolve<TodoQuery>();
            Mutation = resolver.Resolve<TodoMutation>();
        }
    }
}
