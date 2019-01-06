using System;
using ConsoleInDocker;
using Messaging.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Reads.Synchronizer
{
    class Program
    {
        static void Main()
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .AddEnvironmentVariables()
                    .Build();

                var serviceProvider = new ServiceCollection()
                    .AddLogging(options => options.AddConsole())
                    .AddDatabase()
                    .AddMessaging(configuration)
                    .BuildServiceProvider();

                // Start consuming events by resolving the service
                serviceProvider.GetRequiredService<RabbitMQEventConsumer>();

                Wait.ForShutdown();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
