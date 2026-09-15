using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.LearnerCertificates.Queries.GetCertificatePdf;
using RestaurantTraining.Application.Features.LearnerCertificates.Queries.GetMyCertificates;

namespace RestaurantTraining.API.Controllers;

[Authorize]
[ApiController]
[Route("api/learner/certificates")]
public class LearnerCertificatesController : ControllerBase
{
    private readonly IMediator _mediator;

    public LearnerCertificatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private int? GetCurrentUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    // GET /api/learner/certificates  → the signed-in learner's earned certificates
    [HttpGet]
    public async Task<IActionResult> GetMyCertificates()
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        var result = await _mediator.Send(new GetMyCertificatesQuery { UserId = userId.Value });
        return Ok(result);
    }

    //get certificate pdf
    [HttpGet("{certificateId}/download")]
    public async Task<IActionResult> DownloadCertificate(int certificateId)
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        var result = await _mediator.Send(new GetCertificatePdfQuery
        {
            CertificateId = certificateId,
            UserId = userId.Value
        });

        if (result is null) return NotFound();

        return File(result.FileBytes, "application/pdf", result.FileName);
    }

}
