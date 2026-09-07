using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.LearnerModules.Commands.CompleteLesson;
using RestaurantTraining.Application.Features.LearnerModules.Queries.GetAssignedModule;
using RestaurantTraining.Application.Features.LearnerModules.Queries.GetAssignedModules;

namespace RestaurantTraining.API.Controllers;

[Authorize]
[ApiController]
[Route("api/learner/modules")]
public class LearnerModulesController : ControllerBase
{
    private readonly IMediator _mediator;

    public LearnerModulesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET /api/learner/modules  → active modules assigned to the learner's role
    [HttpGet]
    public async Task<IActionResult> GetAssignedModules()
    {
        var roleName = User.FindFirst(
            System.Security.Claims.ClaimTypes.Role)?.Value;

        if (string.IsNullOrWhiteSpace(roleName))
            return Forbid();

        var result = await _mediator.Send(
            new GetAssignedModulesQuery { RoleName = roleName });

        return Ok(result);
    }

    // GET /api/learner/modules/5  → one assigned module's lesson content
    [HttpGet("{moduleId:int}")]
    public async Task<IActionResult> GetAssignedModule(int moduleId)
    {
        var roleName = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        if (string.IsNullOrWhiteSpace(roleName)) return Forbid();

        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        var result = await _mediator.Send(new GetAssignedModuleQuery
        {
            RoleName = roleName,
            UserId = userId.Value,
            ModuleId = moduleId
        });

        return result is null ? NotFound() : Ok(result);
    }

    // POST /api/learner/modules/lessons/5/complete
    [HttpPost("lessons/{lessonId:int}/complete")]
    public async Task<IActionResult> CompleteLesson(int lessonId)
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        var result = await _mediator.Send(new CompleteLessonCommand
        {
            UserId = userId.Value,
            LessonId = lessonId
        });

        if (result.LessonNotFound)
            return NotFound();

        return Ok(new { lessonId = result.LessonId, isCompleted = result.IsCompleted });
    }

    // Reads the learner's user id from the token (mapped from "sub").
    private int? GetCurrentUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }
}
