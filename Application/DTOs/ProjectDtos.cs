namespace AspnetCoreMvcFull.Application.DTOs;

public sealed record CreateProjectRequest(string Name);

public sealed record RecordExpenseRequest(
    decimal Amount,
    string? Category,
    string? Note,
    DateTime? OccurredAt
);

public sealed record EntryDto(
    Guid Id,
    DateTime OccurredAt,
    decimal Amount,
    string? Category,
    string? Note,
    string Type
);

public sealed record ProjectSummaryDto(
    string Id,
    string Name,
    DateTime CreatedAt,
    decimal Balance,
    IReadOnlyList<EntryDto> Entries
);