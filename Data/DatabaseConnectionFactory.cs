using Microsoft.Data.Sqlite;

namespace FlexPlan.Data
{
    public static class DatabaseConnectionFactory
    {
        private static string _connectionString;

        public static void Initialize(string connectionString)
        {
            _connectionString = connectionString;
        }

        public static SqliteConnection CreateConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
