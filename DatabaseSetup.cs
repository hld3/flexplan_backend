using Microsoft.Data.Sqlite;

/**
 * This is only a class to test the connection with a SQLite database.
 * Will be deleted further in the project.
 */
public class DatabaseSetup
{
    public static void SetupAndTestDatabase(string connectionString)
    {
        // var connectionString = "Data Source=FlexPlan.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var createTableCommand = connection.CreateCommand();
            createTableCommand.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT NOT NULL
                )";
            createTableCommand.ExecuteNonQuery();

            var insertCommand = connection.CreateCommand();
            insertCommand.CommandText = "INSERT INTO Users (name) VALUES ('John Poe')";
            insertCommand.ExecuteNonQuery();

            var selectCommand = connection.CreateCommand();
            selectCommand.CommandText = "SELECT Id, Name FROM Users";

            using (var reader = selectCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"Id: {reader.GetInt32(0)}, Name: {reader.GetString(1)}");
                }
            }
        }
    }
}
