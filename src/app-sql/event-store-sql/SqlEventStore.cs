﻿using System;
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
                    ORDER BY sequence_number";

                command.Parameters.AddWithValue("id", id);

                using (command.OpenConnection(connectionString))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return Deserialize(reader.GetString(0), reader.GetString(1));
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
                        (id, sequence_number, type, body)
                        VALUES
                        (@id, @sequenceNumber{i}, @type{i}, @body{i})");

                    var e = newEvents[i];

                    command.Parameters.AddWithValue($"sequenceNumber{i}", eventsLoaded + i);
                    command.Parameters.AddWithValue($"type{i}", e.GetType().AssemblyQualifiedName);
                    command.Parameters.AddWithValue($"body{i}", Serialize(e));
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