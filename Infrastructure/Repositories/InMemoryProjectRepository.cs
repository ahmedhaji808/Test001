using System.Collections.Concurrent;
using AspnetCoreMvcFull.Domain.Entities;
using AspnetCoreMvcFull.Domain.Ports;
using AspnetCoreMvcFull.Domain.ValueObjects;

namespace AspnetCoreMvcFull.Infrastructure.Repositories;

public sealed class InMemoryProjectRepository : IProjectRepository
{
    private readonly ConcurrentDictionary<ProjectId, Project> _store = new();

    public Task<Project?> FindAsync(ProjectId id, CancellationToken cancellationToken)
    {
        _store.TryGetValue(id, out var project);
        return Task.FromResult(project);
    }

    public Task AddAsync(Project project, CancellationToken cancellationToken)
    {
        _store[project.Id] = project;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Project project, CancellationToken cancellationToken)
    {
        _store[project.Id] = project;
        return Task.CompletedTask;
    }
}