using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.LearnerDashboard.Commands.StartModule;
using RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetCompletedModules;
using RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetExploreModules;
using RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetInProgressModules;
using RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetLearnerDashboard;
using RestaurantTraining.Application.Features.LearnerDashboard.Queries.GetMyModulesOverview;

namespace RestaurantTraining.API.Controllers;

[Authorize]
[ApiController]
[Route("api/learner")]
public class LearnerDashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public LearnerDashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/learner/dashboard
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        var result = await _mediator.Send(
            new GetLearnerDashboardQuery { UserId = userId.Value });

        return Ok(result);
    }

    // GET: api/learner/my-modules/overview  → every assigned module with its state
    [HttpGet("my-modules/overview")]
    public async Task<IActionResult> GetOverview()
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        var result = await _mediator.Send(
            new GetMyModulesOverviewQuery { UserId = userId.Value });

        return Ok(result);
    }

    // GET: api/learner/my-modules/in-progress
    [HttpGet("my-modules/in-progress")]
    public async Task<IActionResult> GetInProgressModules()
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        var result = await _mediator.Send(
            new GetInProgressModulesQuery { UserId = userId.Value });

        return Ok(result);
    }

    // GET: api/learner/my-modules/completed
    [HttpGet("my-modules/completed")]
    public async Task<IActionResult> GetCompletedModules()
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        var result = await _mediator.Send(
            new GetCompletedModulesQuery { UserId = userId.Value });

        return Ok(result);
    }

    // GET: api/learner/explore/modules  → role-assigned modules not started yet
    [HttpGet("explore/modules")]
    public async Task<IActionResult> GetExploreModules()
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        var result = await _mediator.Send(
            new GetExploreModulesQuery { UserId = userId.Value });

        return Ok(result);
    }

    // POST: api/learner/modules/5/start
    [HttpPost("modules/{moduleId:int}/start")]
    public async Task<IActionResult> StartModule(int moduleId)
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        var result = await _mediator.Send(new StartModuleCommand
        {
            UserId = userId.Value,
            ModuleId = moduleId
        });

        if (result.NotAssigned)
        {
            return NotFound(new
            {
                Message = "This module is not assigned to your role."
            });
        }

        return Ok(new
        {
            result.ModuleId,
            result.StartedAt,
            result.LastAccessedAt,
            result.IsCompleted
        });
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        return int.TryParse(userIdClaim, out var userId)
            ? userId
            : null;
    }
}
