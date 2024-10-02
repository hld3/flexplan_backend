using FlexPlan.Model;
using FlexPlan.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FlexPlan.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseController(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var exercises = await _exerciseRepository.GetAllExercisesAsync();
            return Ok(exercises);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var exercise = await _exerciseRepository.GetExerciseByIdAsync(id);
            if (exercise == null)
                return NotFound();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Exercise exercise)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _exerciseRepository.AddExerciseAsync(exercise);
            exercise.Id = id;

            return CreatedAtAction(nameof(Get), new { id = exercise.Id }, exercise);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Exercise exercise)
        {
            if (id != exercise.Id)
                return BadRequest();

            var updated = await _exerciseRepository.UpdateExerciseAsync(exercise);
            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _exerciseRepository.DeleteExerciseAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
