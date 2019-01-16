using System;
using GraphQL.Types;

namespace GraphQL.Configuration.Schema
{
    public class Query : ObjectGraphType<object>
    {
        public Query(IQuery query)
        {
            Name = "Query";

            Field<ListGraphType<TodoItemType>>(
                "todos",
                resolve: _ => query.GetAllAsync());

            Field<TodoItemType>(
                "todo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id", Description = "Id of the todo" }
                ),
                resolve: context =>
                {
                    var id = Guid.Parse(context.GetArgument<string>("id"));

                    return query.GetAsync(id);
                });
        }
    }
}
