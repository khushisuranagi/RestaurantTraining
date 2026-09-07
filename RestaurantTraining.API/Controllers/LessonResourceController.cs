using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.LessonResources.Commands.CreateLessonResource;
using RestaurantTraining.Application.Features.LessonResources.Commands.DeleteLessonResource;
using RestaurantTraining.Application.Features.LessonResources.Commands.UpdateLessonResource;
using RestaurantTraining.Application.Features.LessonResources.Queries.GetLessonResources;

namespace RestaurantTraining.API.Controllers
{
    [Authorize(Roles = "Content Creator")]
    [ApiController]
    [Route("api/[controller]")]
    public class LessonResourcesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LessonResourcesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST /api/LessonResources
        [HttpPost]
        public async Task<IActionResult> CreateLessonResource(
            [FromBody] CreateLessonResourceCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // GET /api/LessonResources
        [HttpGet]
        public async Task<IActionResult> GetLessonResources()
        {
            var resources =
                await _mediator.Send(
                    new GetLessonResourcesQuery());

            return Ok(resources);
        }

        // PUT /api/LessonResources/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLessonResource(
            int id,
            [FromBody] UpdateLessonResourceCommand command)
        {
            command.ResourceId = id;

            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // DELETE /api/LessonResources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLessonResource(int id)
        {
            var command = new DeleteLessonResourceCommand
            {
                ResourceId = id
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