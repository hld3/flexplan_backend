using FlexPlan.Controller;
using FlexPlan.Data;
using FlexPlan.Model;
using FlexPlan.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace FlexPlan.Tests.Controller
{
    public class ExerciseControllerTest : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly IExerciseRepository _exerciseRepository;
        private readonly ExerciseController _exerciseController;

        public ExerciseControllerTest()
        {
            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = "file:ControllerTests?mode=memory&cache=shared",
                Mode = SqliteOpenMode.Memory,
                Cache = SqliteCacheMode.Shared
            }.ToString();
            _connection = new SqliteConnection(connectionString);
            _connection.Open();

            InitializeDatabase();

            DatabaseConnectionFactory.Initialize(connectionString);

            _exerciseRepository = new ExerciseRepository();
            _exerciseController = new ExerciseController(_exerciseRepository);
        }

        [Fact]
        public async Task Get_All_Exercises()
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

            await _exerciseRepository.AddExerciseAsync(exercise);
            var result = await _exerciseController.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<Exercise>>(okResult.Value);

            var exercises = new List<Exercise>(returnValue);
            Assert.NotEmpty(exercises);
            // TODO finish testing here.
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
