using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.LearnerScenario.Commands.ReplyToScenario;
using RestaurantTraining.Application.Features.LearnerScenario.Queries.GetScenario;

namespace RestaurantTraining.API.Controllers;

[Authorize]
[ApiController]
[Route("api/learner/scenarios")]
public class LearnerScenarioController : ControllerBase
{
    private readonly IMediator _mediator;

    public LearnerScenarioController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private int? GetCurrentUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    private string? GetRoleName()
        => User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

    // GET /api/learner/scenarios/module/5
    // Gets (or generates + stores) the scenario for the learner's role + module
    [HttpGet("module/{moduleId:int}")]
    public async Task<IActionResult> GetScenario(int moduleId)
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        var roleName = GetRoleName();
        if (string.IsNullOrWhiteSpace(roleName)) return Forbid();

        var result = await _mediator.Send(new GetScenarioQuery
        {
            ModuleId = moduleId,
            RoleName = roleName,
            UserId = userId.Value
        });

        if (result.NotAvailable || result.Scenario is null)
            return NotFound(new { Message = "A practice scenario could not be prepared for this module." });

        return Ok(result.Scenario);
    }

    // POST /api/learner/scenarios/5/reply   body: { "messages": [ ... ] }
    [HttpPost("{scenarioId:int}/reply")]
    public async Task<IActionResult> Reply(int scenarioId, [FromBody] ReplyToScenarioCommand command)
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        command.ScenarioId = scenarioId;
        command.UserId = userId.Value;

        var result = await _mediator.Send(command);

        if (result.NotAvailable)
            return NotFound(new { Message = "This scenario is not available." });

        return Ok(new
        {
            result.Reply,
            result.Feedback,
            result.Ended,
            result.Passed,
            result.CertificateIssued
        });
    }
}
