using AspnetCoreMvcFull.Domain.Entities;
using AspnetCoreMvcFull.Domain.ValueObjects;

namespace AspnetCoreMvcFull.Domain.Ports;

public interface IProjectRepository
{
  Task<Project?> FindAsync(ProjectId id, CancellationToken cancellationToken);
  Task AddAsync(Project project, CancellationToken cancellationToken);
  Task UpdateAsync(Project project, CancellationToken cancellationToken);
}