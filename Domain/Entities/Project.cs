using AspnetCoreMvcFull.Domain.ValueObjects;

namespace AspnetCoreMvcFull.Domain.Entities;

public sealed class Project
{
  private readonly List<Entry> _entries = new();

  public ProjectId Id { get; }
  public string Name { get; private set; }
  public DateTime CreatedAt { get; }

  public IReadOnlyList<Entry> Entries => _entries;

  private Project(ProjectId id, string name, DateTime createdAt)
  {
    Id = id;
    Name = name;
    CreatedAt = createdAt;
  }

  public static Project Create(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
      throw new ArgumentException("Project name is required", nameof(name));
    return new Project(ProjectId.New(), name.Trim(), DateTime.UtcNow);
  }

  public void RecordExpense(decimal amount, string? category, string? note, DateTime? occurredAt = null)
  {
    if (amount <= 0m) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive");
    var entry = new Entry(Guid.NewGuid(), occurredAt ?? DateTime.UtcNow, amount, EntryType.Expense, category, note);
    _entries.Add(entry);
  }

  public decimal GetBalance()
  {
    decimal income = _entries.Where(e => e.Type == EntryType.Income).Sum(e => e.Amount);
    decimal expenses = _entries.Where(e => e.Type == EntryType.Expense).Sum(e => e.Amount);
    return income - expenses;
  }
}