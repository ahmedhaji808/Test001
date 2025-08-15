using AspnetCoreMvcFull.Application.DTOs;
using AspnetCoreMvcFull.Domain.Entities;
using AspnetCoreMvcFull.Domain.Ports;
using AspnetCoreMvcFull.Domain.ValueObjects;

namespace AspnetCoreMvcFull.Application.Services;

public sealed class ProjectAccountingService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectAccountingService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<Project> CreateProjectAsync(string name, CancellationToken cancellationToken)
    {
        var project = Project.Create(name);
        await _projectRepository.AddAsync(project, cancellationToken);
        return project;
    }

    public async Task<bool> RecordExpenseAsync(ProjectId projectId, decimal amount, string? category, string? note, DateTime? occurredAt, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.FindAsync(projectId, cancellationToken);
        if (project is null) return false;
        project.RecordExpense(amount, category, note, occurredAt);
        await _projectRepository.UpdateAsync(project, cancellationToken);
        return true;
    }

    public Task<Project?> GetProjectAsync(ProjectId projectId, CancellationToken cancellationToken)
    {
        return _projectRepository.FindAsync(projectId, cancellationToken);
    }

    public async Task<ProjectSummaryDto?> GetSummaryAsync(ProjectId projectId, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.FindAsync(projectId, cancellationToken);
        if (project is null) return null;

        var entries = project.Entries
            .OrderByDescending(e => e.OccurredAt)
            .Select(e => new EntryDto(
                e.Id,
                e.OccurredAt,
                e.Amount,
                e.Category,
                e.Note,
                e.Type.ToString()
            ))
            .ToList();

        var dto = new ProjectSummaryDto(
            project.Id.ToString(),
            project.Name,
            project.CreatedAt,
            project.GetBalance(),
            entries
        );
        return dto;
    }
}