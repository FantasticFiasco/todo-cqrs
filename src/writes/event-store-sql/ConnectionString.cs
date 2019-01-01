namespace EventStore.Sql
{
    public class ConnectionString
    {
        private readonly string connectionString;

        public ConnectionString(string host, string username, string password)
        {
            connectionString = $"Host={host};Username={username};Password={password}";
        }

        public static implicit operator string(ConnectionString self)
        {
            return self.connectionString;
        }
    }
}
