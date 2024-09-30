using Microsoft.Data.Sqlite;
using FlexPlan.Repository;
using FlexPlan.Data;
using FlexPlan.Model;
using Xunit.Abstractions;

namespace FlexPlan.Tests.Repository
{
    public class ExerciseRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ITestOutputHelper _output;

        public ExerciseRepositoryTests(ITestOutputHelper output)
        {
            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = "file:InMemoryDb?mode=memory&cache=shared",
                Mode = SqliteOpenMode.Memory,
                Cache = SqliteCacheMode.Shared,
                // Uri = true // does not contain a definition for Uri.
            }.ToString();
            _connection = new SqliteConnection(connectionString);
            _connection.Open();
            _output = output;

            InitializeDatabase();

            DatabaseConnectionFactory.Initialize(connectionString);

            _exerciseRepository = new ExerciseRepository();
        }

        [Fact]
        public async Task AddExerciseAsync_Should_Add_Exercise()
        {
            var exercise = new Exercise
            {
                Name = "Test Exercise",
                Description = "Test Description",
                Instructions = "Test Instructions",
                Equipment = "None",
                MuscleGroup = "Full Body",
                VideoUrl = "http://example.com",
                Category = ExerciseCategory.Aerobic
            };

            var id = await _exerciseRepository.AddExerciseAsync(exercise);
            var retrievedExercise = await _exerciseRepository.GetExerciseByIdAsync(id);

            Assert.NotNull(retrievedExercise);
            Assert.Equal(exercise.Name, retrievedExercise.Name);
            Assert.Equal(exercise.Description, retrievedExercise.Description);
            Assert.Equal(exercise.Instructions, retrievedExercise.Instructions);
            Assert.Equal(exercise.Equipment, retrievedExercise.Equipment);
            Assert.Equal(exercise.MuscleGroup, retrievedExercise.MuscleGroup);
            Assert.Equal(exercise.VideoUrl, retrievedExercise.VideoUrl);
            Assert.Equal(exercise.Category, retrievedExercise.Category);
        }

        private void InitializeDatabase()
        {
            using var command = _connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE Exercise (
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

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
