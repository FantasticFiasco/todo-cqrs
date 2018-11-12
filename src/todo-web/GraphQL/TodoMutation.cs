using System;
using Cqrs;
using GraphQL.Types;

namespace Todo.Web.GraphQL
{
    public class TodoMutation : ObjectGraphType
    {
        public TodoMutation(MessageDispatcher messageDispatcher)
        {
            Name = "Mutation";

            Field<AddTodoType>(
                "createTodo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "title" }
                ),
                resolve: context =>
                {
                    var id = Guid.NewGuid();
                    var title = context.GetArgument<string>("title");

                    var command = new AddTodo(id, title);
                    messageDispatcher.SendCommand(command);

                    return command;
                });

            Field<RenameTodoType>(
                "renameTodo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" },
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "newTitle" }
                ),
                resolve: context =>
                {
                    var id = Guid.Parse(context.GetArgument<string>("id"));
                    var newTitle = context.GetArgument<string>("newTitle");

                    var command = new RenameTodo(id, newTitle);
                    messageDispatcher.SendCommand(command);

                    return command;
                });

            Field<CompleteTodoType>(
                "completeTodo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" }
                ),
                resolve: context =>
                {
                    var id = Guid.Parse(context.GetArgument<string>("id"));

                    var command = new CompleteTodo(id);
                    messageDispatcher.SendCommand(command);

                    return command;
                });

            Field<IncompleteTodoType>(
                "incompleteTodo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" }
                ),
                resolve: context =>
                {
                    var id = Guid.Parse(context.GetArgument<string>("id"));

                    var command = new IncompleteTodo(id);
                    messageDispatcher.SendCommand(command);

                    return command;
                });

            Field<RemoveTodoType>(
                "removeTodo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "id" }
                ),
                resolve: context =>
                {
                    var id = Guid.Parse(context.GetArgument<string>("id"));

                    var command = new RemoveTodo(id);
                    messageDispatcher.SendCommand(command);

                    return command;
                });
        }
    }
}
