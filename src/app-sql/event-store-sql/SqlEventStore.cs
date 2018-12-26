using System;
using System.Collections.Generic;
using System.Text;
using Cqrs;
using Newtonsoft.Json;
using Npgsql;

namespace EventStore.Sql
{
    public class SqlEventStore : IEventStore
    {
        private readonly string connectionString;

        public SqlEventStore(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<object> LoadEventsFor<TAggregate>(Guid id)
        {
            using (var command = new NpgsqlCommand())
            {
                command.CommandText = @"
                    SELECT body, type
                    FROM event
                    WHERE id = @id
                    ORDER BY version";

                command.Parameters.AddWithValue("id", id);

                using (command.OpenConnection(connectionString))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return JsonConvert.DeserializeObject(
                            reader.GetString(0),
                            Type.GetType(reader.GetString(1)));
                    }
                }
            }
        }

        public void SaveEventsFor<TAggregate>(Guid id, int eventsLoaded, object[] newEvents)
        {
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

        private static string Serialize(object obj) =>
            ;

        private static object Deserialize(string body, string typeName) =>
            ;
    }
}
