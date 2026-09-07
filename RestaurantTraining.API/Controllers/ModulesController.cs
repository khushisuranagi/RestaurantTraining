using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.Modules.Commands.CreateModule;
using RestaurantTraining.Application.Features.Modules.Commands.DeleteModule;
using RestaurantTraining.Application.Features.Modules.Commands.UpdateModule;
using RestaurantTraining.Application.Features.Modules.Queries.GetModules;

namespace RestaurantTraining.API.Controllers
{
    // Only logged-in users with the Content Creator role can use these endpoints.
    [Authorize(Roles = "Content Creator")]
    [ApiController]
    [Route("api/[controller]")]
    public class ModulesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ModulesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/Modules
        [HttpGet]
        public async Task<IActionResult> GetModules()
        {
            var modules = await _mediator.Send(new GetModulesQuery());
            return Ok(modules);
        }

        // POST /api/Modules
        [HttpPost]
        public async Task<IActionResult> CreateModule(
            [FromBody] CreateModuleCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // PUT /api/Modules/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateModule(
            int id,
            [FromBody] UpdateModuleCommand command)
        {
            // Use the id from the URL so it always matches.
            command.ModuleId = id;

            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // DELETE /api/Modules/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var command = new DeleteModuleCommand { ModuleId = id };

            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }

}
