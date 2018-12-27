using System;
using System.Data;
using System.Data.Common;
using Npgsql;

namespace EventStore.Sql
{
    public static class Extensions
    {
        public static IDbConnection OpenConnection(this DbCommand self, string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

            self.Connection = new NpgsqlConnection(connectionString);
            self.Connection.Open();

            return self.Connection;
        }
    }
}
