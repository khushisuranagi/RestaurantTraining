using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.RoleModules.Commands.AssignModuleToRole;
using RestaurantTraining.Application.Features.RoleModules.Commands.UnassignModuleFromRole;
using RestaurantTraining.Application.Features.RoleModules.Queries.GetModuleAssignments;

namespace RestaurantTraining.API.Controllers
{
    [Authorize(Roles = "Content Creator")]
    [ApiController]
    [Route("api/[controller]")]
    public class RoleModulesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleModulesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/RoleModules/module/5  → roles this module is assigned to
        [HttpGet("module/{moduleId:int}")]
        public async Task<IActionResult> GetForModule(int moduleId)
        {
            var result = await _mediator.Send(
                new GetModuleAssignmentsQuery { ModuleId = moduleId });
            return Ok(result);
        }

        // POST /api/RoleModules  { "roleId": 9, "moduleId": 3 }
        [HttpPost]
        public async Task<IActionResult> Assign(
            [FromBody] AssignModuleToRoleCommand command)
        {
            var response = await _mediator.Send(command);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        // DELETE /api/RoleModules/role/9/module/3
        [HttpDelete("role/{roleId:int}/module/{moduleId:int}")]
        public async Task<IActionResult> Unassign(int roleId, int moduleId)
        {
            var response = await _mediator.Send(
                new UnassignModuleFromRoleCommand { RoleId = roleId, ModuleId = moduleId });
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }
    }
}