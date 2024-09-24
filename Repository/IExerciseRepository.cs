using FlexPlan.Model;

namespace FlexPlan.Repository
{
    public interface IExerciseRepository
    {
        // Create
        Task<int> AddExerciseAsync(Exercise exercise);

        // Read
        Task<Exercise> GetExerciseByIdAsync(int id);
        Task<IEnumerable<Exercise>> GetAllExercisesAsync();

        // Update
        Task<bool> UpdateExerciseAsync(Exercise exercise);

        // Delete
        Task<bool> DeleteExerciseAsync(int id);
    }
}
