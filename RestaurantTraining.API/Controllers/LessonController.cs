using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.Lessons.Commands.CreateLesson;
using RestaurantTraining.Application.Features.Lessons.Commands.DeleteLesson;
using RestaurantTraining.Application.Features.Lessons.Commands.UpdateLesson;
using RestaurantTraining.Application.Features.Lessons.Queries.GetLessons;

namespace RestaurantTraining.API.Controllers
{
    [Authorize(Roles = "Content Creator")]
    [ApiController]
    [Route("api/[controller]")]
    public class LessonsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LessonsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST /api/Lessons
        [HttpPost]
        public async Task<IActionResult> CreateLesson(
            [FromBody] CreateLessonCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // GET /api/Lessons
        [HttpGet]
        public async Task<IActionResult> GetLessons()
        {
            var lessons = await _mediator.Send(new GetLessonsQuery());

            return Ok(lessons);
        }

        // PUT /api/Lessons/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(
            int id,
            [FromBody] UpdateLessonCommand command)
        {
            command.LessonId = id;

            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // DELETE /api/Lessons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var command = new DeleteLessonCommand
            {
                LessonId = id
            };

            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}