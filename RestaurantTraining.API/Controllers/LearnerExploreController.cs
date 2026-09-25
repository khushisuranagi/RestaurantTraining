using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.LearnerExplore.Queries.GetExploreModule;
using RestaurantTraining.Application.Features.LearnerExplore.Queries.GetExploreModules;

namespace RestaurantTraining.API.Controllers;

[Authorize]
[ApiController]
[Route("api/learner/explore")]
public class LearnerExploreController : ControllerBase
{
    private readonly IMediator _mediator;

    public LearnerExploreController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET /api/learner/explore  → active modules NOT assigned to the learner's role
    [HttpGet]
    public async Task<IActionResult> GetExploreModules()
    {
        var roleName = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        if (string.IsNullOrWhiteSpace(roleName)) return Forbid();

        var userClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userClaim, out var userId)) return Forbid();

        var result = await _mediator.Send(
            new GetExploreModulesQuery { RoleName = roleName, UserId = userId });

        return Ok(result);
    }

    // GET /api/learner/explore/5  → read-only lesson content for any active module
    [HttpGet("{moduleId:int}")]
    public async Task<IActionResult> GetExploreModule(int moduleId)
    {
        var result = await _mediator.Send(
            new GetExploreModuleQuery { ModuleId = moduleId });

        return result is null ? NotFound() : Ok(result);
    }
}
