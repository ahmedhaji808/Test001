using AspnetCoreMvcFull.Application.DTOs;
using AspnetCoreMvcFull.Application.Services;
using AspnetCoreMvcFull.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace AspnetCoreMvcFull.Controllers;

[ApiController]
[Route("api/pa")] 
public sealed class ProjectAccountingController : ControllerBase
{
    private readonly ProjectAccountingService _service;

    public ProjectAccountingController(ProjectAccountingService service)
    {
        _service = service;
    }

    [HttpPost("projects")]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return BadRequest(new { message = "Name is required" });

        var project = await _service.CreateProjectAsync(request.Name, cancellationToken);
        return CreatedAtAction(nameof(GetProject), new { id = project.Id.ToString() }, new { id = project.Id.ToString(), name = project.Name });
    }

    [HttpGet("projects/{id}")]
    public async Task<IActionResult> GetProject([FromRoute] string id, CancellationToken cancellationToken)
    {
        if (!ProjectId.TryParse(id, out var projectId))
            return BadRequest(new { message = "Invalid id" });

        var summary = await _service.GetSummaryAsync(projectId, cancellationToken);
        if (summary is null) return NotFound();
        return Ok(summary);
    }

    [HttpPost("projects/{id}/expenses")]
    public async Task<IActionResult> RecordExpense([FromRoute] string id, [FromBody] RecordExpenseRequest request, CancellationToken cancellationToken)
    {
        if (!ProjectId.TryParse(id, out var projectId))
            return BadRequest(new { message = "Invalid id" });
        if (request.Amount <= 0m)
            return BadRequest(new { message = "Amount must be positive" });

        var ok = await _service.RecordExpenseAsync(projectId, request.Amount, request.Category, request.Note, request.OccurredAt, cancellationToken);
        if (!ok) return NotFound();
        return NoContent();
    }
}