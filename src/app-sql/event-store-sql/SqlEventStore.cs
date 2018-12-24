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
                    SELECT type, body
                    FROM event
                    WHERE id = @id
                    ORDER BY version";

                command.Parameters.AddWithValue("id", id);

                using (command.OpenConnection(connectionString))
                using (var reader = command.ExecuteReader())
                {
                    var events = new List<object>();

                    while (reader.Read())
                    {
                        events.Add(Deserialize(reader.GetString(0), reader.GetString(1)));
                    }

                    return events;
                }
            }
        }

        public void SaveEventsFor<TAggregate>(Guid id, int version, object[] newEvents)
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

                    command.Parameters.AddWithValue($"version{i}", version + i);
                    command.Parameters.AddWithValue($"type{i}", newEvents[i].GetType().AssemblyQualifiedName);
                    command.Parameters.AddWithValue($"body{i}", Serialize(newEvents[i]));
                }

                using (command.OpenConnection(connectionString))
                {
                    command.CommandText = commandTextBuilder.ToString();
                    command.ExecuteNonQuery();
                }
            }
        }

        private static string Serialize(object obj) =>
            JsonConvert.SerializeObject(obj);

        private static object Deserialize(string typeName, string body) =>
            JsonConvert.DeserializeObject(body, Type.GetType(typeName));
    }
}
