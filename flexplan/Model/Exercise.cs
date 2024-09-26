using System.ComponentModel.DataAnnotations;

namespace FlexPlan.Model
{
    public class Exercise
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Instructions { get; set; }
        public string Equipment { get; set; }
        public string MuscleGroup { get; set; }
        public string VideoUrl { get; set; }

        [Required]
        public ExerciseCategory Category { get; set; }
    }

    public enum ExerciseCategory
    {
        Aerobic,
        Anaerobic,
        Flexibility,
        Balance
    }
}
