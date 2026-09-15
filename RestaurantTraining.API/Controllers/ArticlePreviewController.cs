using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.ArticlePreview.Queries.GetArticlePreview;

namespace RestaurantTraining.API.Controllers;

[Authorize]
[ApiController]
[Route("api/article-preview")]
public class ArticlePreviewController : ControllerBase
{
    private readonly IMediator _mediator;

    public ArticlePreviewController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET /api/article-preview?url=https://...
    [HttpGet]
    public async Task<IActionResult> GetPreview([FromQuery] string url)
    {
        var result = await _mediator.Send(new GetArticlePreviewQuery { Url = url });
        return Ok(result);
    }
}