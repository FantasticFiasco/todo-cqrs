using System;
using GraphQL.Types;
using Todo;

namespace GraphQL.Configuration.Schema
{
    public class Mutation : ObjectGraphType
    {
        public Mutation(IMutation mutation)
        {
            Name = "Mutation";

            FieldAsync<AddTodoType>(
                "addTodo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "title" }
                ),
                resolve: async context =>
                {
                    var id = Guid.NewGuid();
                    var title = context.GetArgument<string>("title");

                    var command = new AddTodo
                    {
                        Id = id,
                        Title = title
                    };

                    await mutation.HandleAddAsync(command);

                    return command;
                });

            FieldAsync<RenameTodoType>(
                "renameTodo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "newTitle" }
                ),
                resolve: async context =>
                {
                    var id = Guid.Parse(context.GetArgument<string>("id"));
                    var newTitle = context.GetArgument<string>("newTitle");

                    var command = new RenameTodo
                    {
                        Id = id,
                        NewTitle = newTitle
                    };

                    await mutation.HandleRenameAsync(command);

                    return command;
                });

            FieldAsync<CompleteTodoType>(
                "completeTodo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" }
                ),
                resolve: async context =>
                {
                    var id = Guid.Parse(context.GetArgument<string>("id"));

                    var command = new CompleteTodo
                    {
                        Id = id
                    };

                    await mutation.HandleCompleteAsync(command);

                    return command;
                });

            FieldAsync<IncompleteTodoType>(
                "incompleteTodo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" }
                ),
                resolve: async context =>
                {
                    var id = Guid.Parse(context.GetArgument<string>("id"));

                    var command = new IncompleteTodo
                    {
                        Id = id
                    };

                    await mutation.HandleIncompleteAsync(command);

                    return command;
                });

            FieldAsync<RemoveTodoType>(
                "removeTodo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" }
                ),
                resolve: async context =>
                {
                    var id = Guid.Parse(context.GetArgument<string>("id"));

                    var command = new RemoveTodo
                    {
                        Id = id
                    };

                    await mutation.HandleRemoveAsync(command);

                    return command;
                });
        }
    }
}
