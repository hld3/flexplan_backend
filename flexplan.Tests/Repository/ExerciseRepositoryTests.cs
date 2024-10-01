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
        public async Task Add_And_GetExerciseAsync_Should_Add_And_Get_Exercise()
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

        [Fact]
        public async Task UpdateExerciseAsync_Should_Update_Exercise()
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

            exercise.Id = id;
            exercise.Name = "Updated Name";
            bool result = await _exerciseRepository.UpdateExerciseAsync(exercise);
            Assert.True(result);

            Exercise updatedExercise = await _exerciseRepository.GetExerciseByIdAsync(id);
            Assert.NotNull(updatedExercise);
            Assert.Equal(updatedExercise.Name, exercise.Name);
        }

        [Fact]
        public async Task DeleteExerciseAsync()
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
            bool result = await _exerciseRepository.DeleteExerciseAsync(id);
            Assert.True(result);

            Exercise updatedExercise = await _exerciseRepository.GetExerciseByIdAsync(id);
            Assert.Null(updatedExercise);
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
