using System;
using System.Data;
using Npgsql;

namespace EventStore.Sql
{
    public class Schema
    {
        private readonly string connectionString;

        public Schema(string connectionString)
        {
            this.connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public void Create()
        {
            using (var command = new NpgsqlCommand())
            using (command.OpenConnection(connectionString))
            {
                CreateTable(command);
                CreateIndex(command);
            }
        }

        private static void CreateTable(IDbCommand command)
        {
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS event (
                    id uuid NOT NULL,
                    version bigint,
                    type text,
                    body text,
                    created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
                )";

            command.ExecuteNonQuery();
        }

        private static void CreateIndex(IDbCommand command)
        {
            command.CommandText = "CREATE INDEX IF NOT EXISTS id_idx ON event (id)";

            command.ExecuteNonQuery();
        }
    }
}
