using GraphQL.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseTodoGraphQL();
        }
    }
}
