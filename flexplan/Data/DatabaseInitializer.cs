namespace FlexPlan.Data
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            using var connection = DatabaseConnectionFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Exercise (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL,
                    Description TEXT,
                    Instructions TEXT,
                    Equipment TEXT,
                    MuscleGroup TEXT,
                    VideoUrl TEXT,
                    Category TEXT NOT NULL
                );
            ";
            command.ExecuteNonQuery();
        }
    }
}
