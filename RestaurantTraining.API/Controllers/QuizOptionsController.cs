using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.QuizOptions.Commands.CreateQuizOption;
using RestaurantTraining.Application.Features.QuizOptions.Queries.GetQuizOptions;
using RestaurantTraining.Application.Features.QuizOptions.Commands.UpdateQuizOption;
using RestaurantTraining.Application.Features.QuizOptions.Commands.DeleteQuizOption;

namespace RestaurantTraining.API.Controllers
{
    [Authorize(Roles = "Content Creator")]
    [ApiController]
    [Route("api/[controller]")]
    public class QuizOptionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuizOptionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST /api/QuizOptions
        [HttpPost]
        public async Task<IActionResult> CreateQuizOption(
            [FromBody] CreateQuizOptionCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        // GET /api/QuizOptions
        [HttpGet]
        public async Task<IActionResult> GetQuizOptions()
        {
            var options = await _mediator.Send(
                new GetQuizOptionsQuery());

            return Ok(options);
        }

        // PUT /api/QuizOptions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuizOption(
            int id,
            [FromBody] UpdateQuizOptionCommand command)
        {
            command.OptionId = id;

            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // DELETE /api/QuizOptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuizOption(int id)
        {
            var command = new DeleteQuizOptionCommand
            {
                OptionId = id
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