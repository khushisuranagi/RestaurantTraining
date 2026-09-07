using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.AIScenarios.Commands.CreateAIScenario;
using RestaurantTraining.Application.Features.AIScenarios.Commands.DeleteAIScenario;
using RestaurantTraining.Application.Features.AIScenarios.Commands.UpdateAIScenario;
using RestaurantTraining.Application.Features.AIScenarios.Queries.GetAIScenarios;
//using RestaurantTraining.Application.Features.AIScenarios.Commands.DeleteAIScenario;

namespace RestaurantTraining.API.Controllers
{
    [Authorize(Roles = "Content Creator, Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AIScenariosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AIScenariosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST /api/AIScenarios
        [HttpPost]
        public async Task<IActionResult> CreateAIScenario(
            [FromBody] CreateAIScenarioCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // GET /api/AIScenarios
        [HttpGet]
        public async Task<IActionResult> GetAIScenarios()
        {
            var scenarios = await _mediator.Send(
                new GetAIScenariosQuery());

            return Ok(scenarios);
        }

        // PUT /api/AIScenarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAIScenario(
            int id,
            [FromBody] UpdateAIScenarioCommand command)
        {
            command.ScenarioId = id;

            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // DELETE /api/AIScenarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAIScenario(int id)
        {
            var command = new DeleteAIScenarioCommand
            {
                ScenarioId = id
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