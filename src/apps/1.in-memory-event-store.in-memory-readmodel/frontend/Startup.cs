using GraphQL.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Frontend
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCqrs()
                .AddTodoGraphQL();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseTodoGraphQL();
        }
    }
}
