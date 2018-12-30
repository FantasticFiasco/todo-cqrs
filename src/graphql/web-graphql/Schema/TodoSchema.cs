using GraphQL;

namespace Web.GraphQL.Schema
{
    public class TodoSchema : global::GraphQL.Types.Schema
    {
        public TodoSchema(IDependencyResolver resolver)
            : base(resolver)
        {
            Query = resolver.Resolve<Query>();
            Mutation = resolver.Resolve<Mutation>();
        }
    }
}
