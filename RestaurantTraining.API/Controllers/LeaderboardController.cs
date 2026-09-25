using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.Leaderboard.Queries.GetLeaderboard;

namespace RestaurantTraining.API.Controllers;

[Authorize]
[ApiController]
[Route("api/leaderboard")]
public class LeaderboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public LeaderboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET /api/leaderboard  → top 10 + (for learners) their own rank row
    [HttpGet]
    public async Task<IActionResult> GetLeaderboard()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        int? userId = int.TryParse(claim, out var id) ? id : null;

        var result = await _mediator.Send(new GetLeaderboardQuery
        {
            UserId = userId,
            Take = 10
        });

        return Ok(result);
    }
}