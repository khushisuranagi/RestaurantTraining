using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.ContentCreatorDashboard.Queries.GetContentCreatorDashboard;

namespace RestaurantTraining.API.Controllers;

[Authorize(Roles = "Content Creator")]
[ApiController]
[Route("api/content-creator/dashboard")]
public class ContentCreatorDashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContentCreatorDashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET /api/content-creator/dashboard
    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        var result = await _mediator.Send(new GetContentCreatorDashboardQuery());
        return Ok(result);
    }
}
