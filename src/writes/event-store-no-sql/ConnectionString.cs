namespace EventStore.NoSql
{
    public class ConnectionString
    {
        private readonly string connectionString;

        public ConnectionString(string host, string username, string password)
        {
            connectionString = $"mongodb://{username}:{password}@{host}:27017";
        }

        public static implicit operator string(ConnectionString self)
        {
            return self.connectionString;
        }
    }
}
