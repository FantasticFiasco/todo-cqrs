using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Todo.Web.GraphQL.Schema;

namespace Todo.Web.GraphQL
{
    public static class Extensions
    {
        public static void AddTodoGraphQL(this IServiceCollection services)
        {
            // GraphQL
            services.AddSingleton<IDependencyResolver>(serviceProvider => new FuncDependencyResolver(serviceProvider.GetRequiredService));
            services.AddSingleton<ISchema, TodoSchema>();
            services.AddGraphQL();

            // Queries
            services.AddSingleton<TodoQuery>();
            services.AddSingleton<TodoItemType>();
            services.AddSingleton<IdGraphType>();

            // Mutations
            services.AddSingleton<TodoMutation>();
            services.AddSingleton<AddTodoType>();
            services.AddSingleton<CompleteTodoType>();
            services.AddSingleton<IncompleteTodoType>();
            services.AddSingleton<RemoveTodoType>();
            services.AddSingleton<RenameTodoType>();
            services.AddSingleton<GuidGraphType>();
        }

        public static void UseTodoGraphQL(this IApplicationBuilder app)
        {
            app.UseGraphQL<ISchema>();

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
            {
                Path = "/ui/playground"
            });
        }
    }
}
