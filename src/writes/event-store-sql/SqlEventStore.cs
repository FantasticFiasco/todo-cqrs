using System;
using System.Collections.Generic;
using System.Text;
using Cqrs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;

namespace EventStore.Sql
{
    public class SqlEventStore : IEventStore
    {
        private readonly ConnectionString connectionString;
        private readonly ILogger<SqlEventStore> logger;

        public SqlEventStore(ConnectionString connectionString, ILogger<SqlEventStore> logger)
        {
            this.connectionString = connectionString;
            this.logger = logger;
        }

        public IEnumerable<object> LoadEventsFor<TAggregate>(Guid id)
        {
            logger.LogInformation("Load events for aggregate with id {id}", id);

            using (var command = new NpgsqlCommand())
            {
                command.CommandText = @"
                    SELECT body, type
                    FROM event
                    WHERE id = @id
                    ORDER BY version";

                command.Parameters.AddWithValue("id", id);

                var events = new List<object>();

                using (command.OpenConnection(connectionString))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        events.Add(JsonConvert.DeserializeObject(
                            reader.GetString(0),
                            Type.GetType(reader.GetString(1))));
                    }
                }

                logger.LogInformation("Loaded {count} events for aggregate {id}", events.Count, id);

                return events;
            }
        }

        public void SaveEventsFor<TAggregate>(Guid id, int eventsLoaded, object[] newEvents)
        {
            logger.LogInformation("Save {count} event(s) for aggregate with id {id}", newEvents.Length, id);

            using (var command = new NpgsqlCommand())
            {
                var commandTextBuilder = new StringBuilder();

                // Common parameters
                command.Parameters.AddWithValue("id", id);

                for (var i = 0; i < newEvents.Length; i++)
                {
                    commandTextBuilder.Append($@"
                        INSERT INTO event
                        (id, version, type, body)
                        VALUES
                        (@id, @version{i}, @type{i}, @body{i})");

                    command.Parameters.AddWithValue($"version{i}", eventsLoaded + i);
                    command.Parameters.AddWithValue($"type{i}", newEvents[i].GetType().AssemblyQualifiedName);
                    command.Parameters.AddWithValue($"body{i}", JsonConvert.SerializeObject(newEvents[i]));
                }

                using (command.OpenConnection(connectionString))
                {
                    command.CommandText = commandTextBuilder.ToString();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
