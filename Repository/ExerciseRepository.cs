using Microsoft.Data.Sqlite;
using FlexPlan.Data;
using FlexPlan.Model;

namespace FlexPlan.Repository
{
    public class ExerciseRepository : IExerciseRepository
    {
        public async Task<int> AddExerciseAsync(Exercise exercise)
        {
            using var connection = DatabaseConnectionFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = @"
                INSERT INTO Exercises (Name, Description, Instructions, Equipment, MuscleGroup, VideoUrl, Category)
                VALUES ($name, $description, $instructions, $equipment, $muscleGroup, $videoUrl, $category);
                SELECT last_insert_rowid();
            ";

            command.Parameters.AddWithValue("$name", exercise.Name);
            command.Parameters.AddWithValue("$description", exercise.Description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("$instructions", exercise.Instructions ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("$equipment", exercise.Equipment ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("$muscleGroup", exercise.MuscleGroup ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("$videoUrl", exercise.VideoUrl ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("$category", exercise.Category.ToString());

            var id = (long)await command.ExecuteScalarAsync();
            return (int)id;
        }

        public async Task<Exercise> GetExerciseByIdAsync(int id)
        {
            using var connection = DatabaseConnectionFactory.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, Name, Description, Instructions, Equipment, MuscleGroup, VideoUrl, Category
                FROM Exercises
                WHERE Id = $id;
            ";
            command.Parameters.AddWithValue("$id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReaderToExercise(reader);
            }

            return null;
        }

        private Exercise MapReaderToExercise(SqliteDataReader reader)
        {
            return new Exercise
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                Instructions = reader.IsDBNull(3) ? null : reader.GetString(3),
                Equipment = reader.IsDBNull(4) ? null : reader.GetString(4),
                MuscleGroup = reader.IsDBNull(5) ? null : reader.GetString(5),
                VideoUrl = reader.IsDBNull(6) ? null : reader.GetString(6),
                Category = Enum.Parse<ExerciseCategory>(reader.GetString(7))
            };
        }
    }
}
