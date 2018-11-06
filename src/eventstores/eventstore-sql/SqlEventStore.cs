using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Cqrs;

namespace EventStore.Sql
{
    /// <summary>
    /// This is a simple example implementation of an event store, using a SQL database
    /// to provide the storage. Tested and known to work with SQL Server.
    /// </summary>
    public class SqlEventStore : IEventStore
    {
        private readonly string connectionString;

        public SqlEventStore(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public object[] LoadEventsFor<TAggregate>(Guid id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = @"
                        SELECT [Type], [Body]
                        FROM [dbo].[Events]
                        WHERE [AggregateId] = @AggregateId
                        ORDER BY [SequenceNumber]";
                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@AggregateId", id));

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return DeserializeEvent(reader.GetString(0), reader.GetString(1));
                        }
                    }
                }
            }
        }

        public void SaveEventsFor<TAggregate>(Guid aggregateId, int eventsLoaded, ArrayList newEvents)
        {
            using (var command = new SqlCommand())
            {
                // Query prelude
                var queryText = new StringBuilder(512);
                queryText.AppendLine("BEGIN TRANSACTION;");
                queryText.AppendLine(
                    @"IF NOT EXISTS(SELECT * FROM [dbo].[Aggregates] WHERE [Id] = @AggregateId)
                          INSERT INTO [dbo].[Aggregates] ([Id], [Type]) VALUES (@AggregateId, @AggregateType);");
                command.Parameters.AddWithValue("AggregateId", aggregateId);
                command.Parameters.AddWithValue("AggregateType", typeof(TAggregate).AssemblyQualifiedName);

                // Add saving of the events
                command.Parameters.AddWithValue("CommitDateTime", DateTime.UtcNow);
                for (int i = 0; i < newEvents.Count; i++)
                {
                    var e = newEvents[i];
                    queryText.AppendFormat(
                        @"INSERT INTO [dbo].[Events] ([AggregateId], [SequenceNumber], [Type], [Body], [Timestamp])
                          VALUES(@AggregateId, {0}, @Type{1}, @Body{1}, @CommitDateTime);",
                        eventsLoaded + i, i);
                    command.Parameters.AddWithValue("Type" + i.ToString(), e.GetType().AssemblyQualifiedName);
                    command.Parameters.AddWithValue("Body" + i.ToString(), SerializeEvent(e));
                }

                // Add commit
                queryText.Append("COMMIT;");

                // Execute the update
                using (var con = new SqlConnection(connectionString))
                {
                    con.Open();
                    command.Connection = con;
                    command.CommandText = queryText.ToString();
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
        }

        private static object DeserializeEvent(string typeName, string data)
        {
            var ser = new XmlSerializer(Type.GetType(typeName));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(data));
            ms.Seek(0, SeekOrigin.Begin);
            return ser.Deserialize(ms);
        }

        private static string SerializeEvent(object obj)
        {
            var ser = new XmlSerializer(obj.GetType());
            var ms = new MemoryStream();

            ser.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);

            return new StreamReader(ms).ReadToEnd();
        }
    }
}
